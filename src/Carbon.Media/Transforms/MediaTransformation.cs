﻿namespace Carbon.Media
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Text;

	public class MediaTransformation
	{
		protected readonly IMediaInfo source;
		protected readonly string format;
		protected readonly List<ITransform> transforms = new List<ITransform>();

		private Size size;

		private Uri baseUri;

		public MediaTransformation(IMediaInfo source, string format)
		{
			#region Preconditions

			if (source == null) throw new ArgumentNullException("source");
			if (format == null) throw new ArgumentNullException("format");

			#endregion

			this.source = source;
			this.format = format;

			this.size = new Size(source.Width, source.Height);
		}


		public MediaTransformation(IMediaInfo source, MediaOrientation orientation, string format)
			: this(source, format)
		{
			Transform(orientation.GetTransforms());	
		}

		public IMediaInfo Source
		{
			get { return source; }
		}

		public string Format
		{
			get { return format; }
		}

		// e.g. http://media.io/

		public Uri BaseUri
		{
			get { return baseUri; }
			set { baseUri = value; }
		}

		public Size Size
		{
			get { return size; }
		}

		// TODO: Immutable
		public IList<ITransform> GetTransforms()
		{
			return transforms.AsReadOnly();
		}

		public MediaTransformation Transform(params ITransform[] transformList)
		{
			#region Preconditions

			if (transformList == null) throw new ArgumentNullException("transformList");

			#endregion

			if (transformList.Length == 0) return this;

			foreach (var transform in transformList)
			{
				#region Update the Current Size

				if (transform is AnchoredResize)
				{
					var anchoredResize = (AnchoredResize)transform;

					size = anchoredResize.Size;
				}
				else if (transform is Resize)
				{
					var resize = (Resize)transform;

					size = resize.Size;
				}
				else if (transform is Crop)
				{
					var crop = (Crop)transform;

					size = crop.Rectangle.Size;
				}
				else if (transform is Rotate)
				{
					var rotate = (Rotate)transform;

					// Flip the height & width
					if (rotate.Angle == 90 || rotate.Angle == 270)
					{
						size = new Size(size.Height, size.Width);
					}
				}

				#endregion

				this.transforms.Add(transform);
			}

			return this;
		}

		#region Builders

		public MediaTransformation Rotate(int angle)
		{
			Transform(new Rotate(angle));

			return this;
		}

		public MediaTransformation Crop(int x, int y, int width, int height)
		{
			Transform(new Crop(x, y, width, height));

			return this;
		}

		public MediaTransformation Resize(int width, int height)
		{
			Transform(new Resize(width, height));

			return this;
		}

		public MediaTransformation ApplyFilter(string name, int value)
		{
			Transform(new ApplyFilter(name, value.ToString()));

			return this;
		}

		#endregion

		#region Transform Helpers

		public bool HasTransforms
		{
			get { return transforms.Count > 0; }
		}

		#endregion

		#region Helpers

		public static MediaTransformation ParsePath(string path)
		{
			// 100/transform/transform.format

			var lastDotIndex = path.LastIndexOf('.');
			var format = (lastDotIndex > 0) ? path.Substring(lastDotIndex + 1): null;

			if (lastDotIndex > 0)
			{
				path = path.Substring(0, lastDotIndex);
			}

			var segments = path.Split('/');

			int i = 1;

			var id = 0;
			var transforms = new List<ITransform>();

			foreach (var segment in segments)
			{
				if (i == 1) 
				{
					id = Int32.Parse(segment);
				}
				else
				{
					ITransform transform;

					var transformName = segment.Split('(', ':')[0];

					if (Char.IsDigit(segment[0]))
					{
						if (segment.Contains("-"))
						{
							transform = Carbon.Media.AnchoredResize.Parse(segment);
						}
						else
						{
							transform = Carbon.Media.Resize.Parse(segment);
						}
					}
					else
					{

						switch (transformName)
						{
							case "crop"   : transform = Carbon.Media.Crop.Parse(segment);        break;
							case "rotate" : transform = Carbon.Media.Rotate.Parse(segment);      break;
							case "flip"   : transform = Carbon.Media.Flip.Parse(segment);        break;
							default       : transform = Carbon.Media.ApplyFilter.Parse(segment); break;
						}
					}

					transforms.Add(transform);
				}

				i++;
			}

			var rendition = new MediaTransformation(new MediaMock { Id = id }, format);

			foreach (var t in transforms)
			{
				rendition.Transform(t);
			}

			return rendition;
		}

		/*
		public string GetKey()
		{
			return source.Id + ":" + GetFullName();
		}
		*/

		public string GetPath()
		{
			return source.Id + "/" + GetFullName();
		}

		public string GetFullName()
		{
			/* 
			10x10.gif			
			crop:0-0_10x10.jpeg		// A cropped image rendention (x=0,y=0,width=100,height=100)
			10x10-c/rotate(90).png	// A 10x10 image (anchored at it's center when resized) rotated 90 degrees
			200x100/rotate(90).png
			640x480.mp4
			*/

			var sb = new StringBuilder();

			foreach (var transform in transforms)
			{
				if(sb.Length != 0)
				{
					sb.Append("/");
				}

				sb.Append(transform.ToString());
			}

			sb.Append(".");

			sb.Append(format);

			return sb.ToString();
		}

		#endregion

		public Uri Url
		{
			get { return new Uri(baseUri, GetPath()); }
		}
	}

	internal class MediaMock : IMediaInfo
	{
		public int Id { get; set; }

		public byte[] Sha256 { get; set; }

		public string Format
		{
			get { return null; }
		}

		public int Width
		{
			get { return 0; }
		}

		public int Height
		{
			get { return 0; }
		}
	}
}
