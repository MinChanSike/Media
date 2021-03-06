﻿namespace Carbon.Media.Codecs
{
    public sealed class HevcEncoder : Encoder
    {
        private readonly HevcEncodingOptions options;

        public HevcEncoder(HevcEncodingOptions options)
            : base(CodecId.Hevc)
        {
            this.options = options;
        }
    }
}
