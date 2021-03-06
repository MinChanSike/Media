﻿using System.Collections.Generic;

namespace Carbon.Media.Metadata.Exif
{
    using static ExifTagFormat;
    using static ExifTag;

    public class ExifTagMap
    {
        private static readonly Dictionary<ExifTag, ExifTagInfo> TagsToTypes = new Dictionary<ExifTag, ExifTagInfo> {
			// GPS Metadata			
			{ GPSVersionID,              new ExifTagInfo(GPSVersionID,              Byte) },
            { GPSLatitudeRef,            new ExifTagInfo(GPSLatitudeRef,            Ansi) },
            { GPSLatitude,               new ExifTagInfo(GPSLatitude,               Rational) },
            { GPSLongitudeRef,           new ExifTagInfo(GPSLongitudeRef,           Ansi) },
            { GPSLongitude,              new ExifTagInfo(GPSLongitude,              Rational) },
            { GPSAltitudeRef,            new ExifTagInfo(GPSAltitudeRef,            Byte) },
            { GPSAltitude,               new ExifTagInfo(GPSAltitude,               Rational) },
            { GPSTimestamp,              new ExifTagInfo(GPSTimestamp,              Rational) },
            { GPSSatellites,             new ExifTagInfo(GPSSatellites,             Ansi) },
            { GPSStatus,                 new ExifTagInfo(GPSStatus,                 Ansi) },
            { GPSMeasureMode,            new ExifTagInfo(GPSMeasureMode,            Ansi) },
            { GPSDOP,                    new ExifTagInfo(GPSDOP,                    Rational) },
            { GPSSpeedRef,               new ExifTagInfo(GPSSpeedRef,               Ansi) },
            { GPSSpeed,                  new ExifTagInfo(GPSSpeed,                  Rational) },
            { GPSTrackRef,               new ExifTagInfo(GPSTrackRef) },
            { GPSTrack,                  new ExifTagInfo(GPSTrack) },
            { GPSImgDirectionRef,        new ExifTagInfo(GPSImgDirectionRef) },
            { GPSImgDirection,           new ExifTagInfo(GPSImgDirection) },
            { GPSMapDatum,               new ExifTagInfo(GPSMapDatum,               Ansi) },
            { GPSDestLatitudeRef,        new ExifTagInfo(GPSDestLatitudeRef) },
            { GPSDestLatitude,           new ExifTagInfo(GPSDestLatitude) },
            { GPSDestLongitudeRef,       new ExifTagInfo(GPSDestLongitudeRef) },
            { GPSDestLongitude,          new ExifTagInfo(GPSDestLongitude) },
            { GPSDestBearingRef,         new ExifTagInfo(GPSDestBearingRef) },
            { GPSDestBearing,            new ExifTagInfo(GPSDestBearing) },
            { GPSDestDistanceRef,        new ExifTagInfo(GPSDestDistanceRef) },
            { GPSDestDistance,           new ExifTagInfo(GPSDestDistance) },
            { GPSProcessingMethod,       new ExifTagInfo(GPSProcessingMethod) },
            { GPSAreaInformation,        new ExifTagInfo(GPSAreaInformation) },
            { GPSDateStamp,              new ExifTagInfo(GPSDateStamp,              Ansi) },
            { GPSDifferential,           new ExifTagInfo(GPSDifferential,           Short) },

            // Base types
            { SubfileType,               ExifTagInfo.SubfileType },
            // { OldSubfileType,            ExifTagInfo.OldSubfileType },
            // { ImageWidth,                ExifTagInfo.ImageWidth },
            // { ImageLength,               ExifTagInfo.ImageLength },
            // { BitsPerSample,             ExifTagInfo.BitsPerSample },
            { Compression,               ExifTagInfo.Compression },
            { PhotometricInterpretation, ExifTagInfo.PhotometricInterpretation },
            { Thresholding,              ExifTagInfo.Thresholding },
            // { CellWidth,                 ExifTagInfo.CellWidth },
            // { CellLength,                ExifTagInfo.CellLength },
            // { FillOrder,                 ExifTagInfo.FillOrder },
            // { DocumentName,              ExifTagInfo.DocumentName },
            // { ImageDescription,          ExifTagInfo.ImageDescription },
            // { Make,                      ExifTagInfo.Make },
            // { Model,                     ExifTagInfo.Model },
            // { StripOffsets,              ExifTagInfo.StripOffsets },
            { Orientation,               ExifTagInfo.Orientation },
            // { SamplesPerPixel            ExifTagInfo.SamplesPerPixel },
            // { RowsPerStrip,              ExifTagInfo.RowsPerStrip },
            // { StripByteCounts,           ExifTagInfo.StripByteCounts },
            // { MinSampleValue,            ExifTagInfo.MinSampleValue },
            // { MaxSampleValue,            ExifTagInfo.MaxSampleValue },
            { XResolution,               ExifTagInfo.XResolution },
            { YResolution,               ExifTagInfo.YResolution },
            { PlanarConfiguration,       ExifTagInfo.PlanarConfiguration },
            // { PageName,                  ExifTagInfo.PageName },
            // { XPosition,                 ExifTagInfo.XPosition },
            // { YPosition,                 ExifTagInfo.YPosition },
            // { FreeOffsets,               ExifTagInfo.FreeOffsets },
            // { FreeByteCounts,            ExifTagInfo.FreeByteCounts },
            // { GrayResponseUnit,          ExifTagInfo.GrayResponseCurve },
            // { GrayResponseCurve,         ExifTagInfo.GrayResponseCurve },
            // { T4Options,                 ExifTagInfo.T4Options },
            // { T6Options,                 ExifTagInfo.T6Options },
            { ResolutionUnit,            ExifTagInfo.ResolutionUnit },
            // { PageNumber,                ExifTagInfo.PageNumber },
            // { ColorResponseUnit,         ExifTagInfo.ColorResponseUnit },
            // { TransferFunction,          ExifTagInfo.TransferFunction },
            { Software,                  ExifTagInfo.Software },
            // { DateTime,                  ExifTagInfo.DateTime },
            { Artist,                    ExifTagInfo.Artist                   },
            // { HostComputer,              ExifTagInfo.HostComputer             },
            // { Predictor,                 ExifTagInfo.Predictor                },
            // { WhitePoint,                ExifTagInfo.WhitePoint               },
            // { PrimaryChromaticities,     ExifTagInfo.PrimaryChromaticities    },
            // { ColorMap,                  ExifTagInfo.ColorMap                 },
            // { HalftoneHints,             ExifTagInfo.HalftoneHints            },
            // { TileWidth,                 ExifTagInfo.TileWidth                },
            // { TileLength,                ExifTagInfo.TileLength               },
            // { TileOffsets,               ExifTagInfo.TileOffsets              },
            // { TileByteCounts,            ExifTagInfo.TileByteCounts           },
            // { BadFaxLines,               ExifTagInfo.BadFaxLines              },
            // { CleanFaxData,              ExifTagInfo.CleanFaxData             },
            // { ConsecutiveBadFaxLines,    ExifTagInfo.ConsecutiveBadFaxLines   },
            // { SubIFDs,                   ExifTagInfo.SubIFDs                  },
            // { InkSet,                    ExifTagInfo.InkSet                   },
            // { InkNames,                  ExifTagInfo.InkNames                 },
            // { NumberOfInks,              ExifTagInfo.NumberOfInks             },
            // { DotRange,                  ExifTagInfo.DotRange                 },
            // { TargetPrinter,             ExifTagInfo.TargetPrinter,            },
            // { ExtraSamples,              ExifTagInfo.ExtraSamples,             },
            // { SampleFormat,              ExifTagInfo.SampleFormat,             },
            // { SMinSampleValue,           ExifTagInfo.SMinSampleValue,          },
            // { SMaxSampleValue,           ExifTagInfo.SMaxSampleValue,          },
            // { TransferRange,             ExifTagInfo.TransferRange,            },
            // { ClipPath,                  ExifTagInfo.ClipPath,                 },
            // { XClipPathUnits,            ExifTagInfo.XClipPathUnits           },
            // { YClipPathUnits,            ExifTagInfo.YClipPathUnits           },
            // { Indexed,                   ExifTagInfo.Indexed                  },
            // { JPEGTables,                ExifTagInfo.JPEGTables               },
            // { OPIProxy,                  ExifTagInfo.OPIProxy                 },
            // { ProfileType,               ExifTagInfo.ProfileType              },
            { FaxProfile,                ExifTagInfo.FaxProfile },
            // { CodingMethods,             ExifTagInfo.CodingMethods            },
            // { VersionYear,               ExifTagInfo.VersionYear              },
            // { ModeNumber,                ExifTagInfo.ModeNumber               },
            // { Decode,                    ExifTagInfo.Decode                   },
            // { DefaultImageColor,         ExifTagInfo.DefaultImageColor        },
            // { T82ptions,                 ExifTagInfo.T82ptions                },
            // { JPEGProc,                  ExifTagInfo.JPEGProc                 },
            // { JPEGInterchangeFormat,     ExifTagInfo.JPEGInterchangeFormat    },
            // { JPEGInterchangeFormatLen,  ExifTagInfo.JPEGInterchangeFormatLen },
            // { JPEGRestartInterval,       ExifTagInfo.JPEGRestartInterval      },
            // { JPEGLosslessPredictors,    ExifTagInfo.JPEGLosslessPredictors   },
            // { JPEGPointTransforms,       ExifTagInfo.JPEGPointTransforms      },
            // { JPEGQTables,               ExifTagInfo.JPEGQTables              },
            // { JPEGDCTables,              ExifTagInfo.JPEGDCTables             },
            // { JPEGACTables,              ExifTagInfo.JPEGACTables             },
            // { YCbCrCoefficients,         ExifTagInfo.YCbCrCoefficients        },
            // { YCbCrSubSampling,          ExifTagInfo.YCbCrSubSampling         },
            // { YCbCrPositioning,          ExifTagInfo.YCbCrPositioning         },
            // { ReferenceBlackWhite,       ExifTagInfo.ReferenceBlackWhite      },
            // { StripRowCounts,            ExifTagInfo.StripRowCounts           },
            // { XMP,                       ExifTagInfo.XMP                      },
            // { Rating,                    ExifTagInfo.Rating },
            // { RatingPercent,             ExifTagInfo.RatingPercent },
            // { ImageID,                   ExifTagInfo.ImageID },
            // { CFARepeatPatternDim,       ExifTagInfo.CFARepeatPatternDim },
            // { CFAPattern2,               ExifTagInfo.CFAPattern2 },
            // { BatteryLevel,              ExifTagInfo.BatteryLevel },
            { Copyright,                 ExifTagInfo.Copyright     },
            { ExposureTime,              ExifTagInfo.ExposureTime },
            { FNumber,                   ExifTagInfo.FNumber },
            // { MDFileTag,                 ExifTagInfo.MDFileTag },
            // { MDScalePixel,              ExifTagInfo.MDScalePixel },
            // { MDLabName,                 ExifTagInfo.MDLabName },
            // { MDSampleInfo,              ExifTagInfo.MDSampleInfo },
            // { MDPrepDate,                ExifTagInfo.MDPrepDate },
            // { MDPrepTime,                ExifTagInfo.MDPrepTime },
            // { MDFileUnits,               ExifTagInfo.MDFileUnits },
            // { PixelScale,                ExifTagInfo.PixelScale },
            // { IntergraphPacketData       ExifTagInfo.IntergraphPacketData },
            // { IntergraphRegisters        ExifTagInfo.IntergraphRegisters },
            // { IntergraphMatrix           ExifTagInfo.IntergraphMatrix },
            // { ModelTiePoint              ExifTagInfo.ModelTiePoint },
            // { SEMInfo                    ExifTagInfo.SEMInfo },
            // { ModelTransform             ExifTagInfo.ModelTransform },
            // { SubIFDOffset               ExifTagInfo.SubIFDOffset },
            // { ImageLayer                 ExifTagInfo.ImageLayer },
            { ExposureProgram,           ExifTagInfo.ExposureProgram },
            { SpectralSensitivity,       ExifTagInfo.SpectralSensitivity },
            // { GPSIFDOffset,              ExifTagInfo.GPSIFDOffset            },
            { ISOSpeedRatings,           ExifTagInfo.ISOSpeedRatings         },
            // { OECF,                      ExifTagInfo.OECF                    },
            // { Interlace,                 ExifTagInfo.Interlace               },
            // { TimeZoneOffset,            ExifTagInfo.TimeZoneOffset          },
            // { SelfTimerMode,             ExifTagInfo.SelfTimerMode           },
            // { SensitivityType,           ExifTagInfo.SensitivityType         },
            // { StandardOutputSensitivity, ExifTagInfo.StandardOutputSensitivity },
            // { RecommendedExposureIndex,  ExifTagInfo.RecommendedExposureIndex },
            // { ISOSpeed,                  ExifTagInfo.ISOSpeed                },
            // { ISOSpeedLatitudeyyy,       ExifTagInfo.ISOSpeedLatitudeyyy     },
            // { ISOSpeedLatitudezzz,       ExifTagInfo.ISOSpeedLatitudezzz     },
            // { FaxRecvParams              ExifTagInfo.FaxRecvParams            },
            // { FaxSubaddress,             ExifTagInfo.FaxSubaddress           },
            // { FaxRecvTime,               ExifTagInfo.FaxRecvTime             },
            // { ExifVersion,               ExifTagInfo.ExifVersion             },
            // { DateTimeOriginal,          ExifTagInfo.DateTimeOriginal        },
            // { DateTimeDigitized,         ExifTagInfo.DateTimeDigitized       },
            // { OffsetTime,                ExifTagInfo.OffsetTime              },
            // { OffsetTimeOriginal,        ExifTagInfo.OffsetTimeOriginal      },
            // { OffsetTimeDigitized,       ExifTagInfo.OffsetTimeDigitized     },
            // { ComponentsConfiguration,   ExifTagInfo.ComponentsConfiguration },
            // { CompressedBitsPerPixel     ExifTagInfo.CompressedBitsPerPixel   },
            { ShutterSpeedValue,          ExifTagInfo.ShutterSpeedValue        },
            { ApertureValue,              ExifTagInfo.ApertureValue            },
            { BrightnessValue,            ExifTagInfo.BrightnessValue          },
            // { ExposureBiasValue,          ExifTagInfo.ExposureBiasValue        },
            // { MaxApertureValue,           ExifTagInfo.MaxApertureValue         },
            // { SubjectDistance,            ExifTagInfo.SubjectDistance          },
            // { MeteringMode               ExifTagInfo.MeteringMode             },
            // { LightSource                ExifTagInfo.LightSource              },
            // { Flash                      ExifTagInfo.Flash                    },
            // { FocalLength                ExifTagInfo.FocalLength              },
            // { _FlashEnergy               ExifTagInfo._FlashEnergy             },
            // { _SpatialFrequencyRespons   ExifTagInfo._SpatialFrequencyRespons },
            // { Noise                      ExifTagInfo.Noise                    },
            // { _FocalPlaneXResolution     ExifTagInfo._FocalPlaneXResolution   },
            // { _FocalPlaneYResolution     ExifTagInfo._FocalPlaneYResolution   },
            // { FocalPlaneResolutionUnit   ExifTagInfo.FocalPlaneResolutionUnit },
            // { ImageNumber                ExifTagInfo.ImageNumber              },
            // { SecurityClassification     ExifTagInfo.SecurityClassification   },
            // { ImageHistory               ExifTagInfo.ImageHistory             },
            // { SubjectArea                ExifTagInfo.SubjectArea              },
            // { _ExposureIndex             ExifTagInfo._ExposureIndex           },
            // { TIFFEPStandardID           ExifTagInfo.TIFFEPStandardID         },
            // { _SensingMethod             ExifTagInfo._SensingMethod           },
            // { MakerNote                  ExifTagInfo.MakerNote                },
            // { UserComment                ExifTagInfo.UserComment              },
            // { SubsecTime                 ExifTagInfo.SubsecTime               },
            // { SubsecTimeOriginal         ExifTagInfo.SubsecTimeOriginal       },
            // { SubsecTimeDigitized        ExifTagInfo.SubsecTimeDigitized      },
            // { ImageSourceData            ExifTagInfo.ImageSourceData          },
            // { AmbientTemperature         ExifTagInfo.AmbientTemperature       },
            // { Humidity                   ExifTagInfo.Humidity                 },
            // { Pressure                   ExifTagInfo.Pressure                 },
            // { WaterDepth                 ExifTagInfo.WaterDepth               },
            // { Acceleration               ExifTagInfo.Acceleration             },
            // { CameraElevationAngle       ExifTagInfo.CameraElevationAngle     },
            // { XPTitle                    ExifTagInfo.XPTitle                  },
            // { XPComment                  ExifTagInfo.XPComment                },
            // { XPAuthor                   ExifTagInfo.XPAuthor                 },
            // { XPKeywords                 ExifTagInfo.XPKeywords               },
            // { XPSubject                  ExifTagInfo.XPSubject                },
            { ColorSpace,                 ExifTagInfo.ColorSpace               },
            { PixelXDimension,            ExifTagInfo.PixelXDimension          },
            { PixelYDimension,            ExifTagInfo.PixelYDimension          },
            //{ RelatedSoundFile,           ExifTagInfo.RelatedSoundFile         },
            { FlashpixVersion,            ExifTagInfo.FlashpixVersion          },
            { FlashEnergy,                ExifTagInfo.FlashEnergy              },
            // { SpatialFrequencyResponse,   ExifTagInfo.SpatialFrequencyResponse },
            { FocalPlaneXResolution,      ExifTagInfo.FocalPlaneXResolution    },
            { FocalPlaneYResolution,      ExifTagInfo.FocalPlaneYResolution    },
            { FocalPlaneResolutionUnit,   ExifTagInfo.FocalPlaneResolutionUnit },
            { SubjectLocation,            ExifTagInfo.SubjectLocation          },
            { ExposureIndex,              ExifTagInfo.ExposureIndex            },
            { SensingMethod,              ExifTagInfo.SensingMethod            },
            // { FileSource,                 ExifTagInfo.FileSource               },
            { SceneType,                  ExifTagInfo.SceneType                },
            // { CFAPattern,                 ExifTagInfo.CFAPattern               },
            { CustomRendered,             ExifTagInfo.CustomRendered           },
            { ExposureMode,               ExifTagInfo.ExposureMode },
            { WhiteBalance,               ExifTagInfo.WhiteBalance },
            { DigitalZoomRatio,          ExifTagInfo.DigitalZoomRatio },
            { FocalLengthIn35mmFilm,     ExifTagInfo.FocalLengthIn35mmFilm },
            { SceneCaptureType,          ExifTagInfo.SceneCaptureType },
            { GainControl,               ExifTagInfo.GainControl },
            { Contrast,                  ExifTagInfo.Contrast },
            { Saturation,                ExifTagInfo.Saturation },
            { Sharpness,                 ExifTagInfo.Sharpness },
            // { DeviceSettingDescription,  ExifTagInfo.DeviceSettingDescription },
            { SubjectDistanceRange,      ExifTagInfo.SubjectDistanceRange },
            // { ImageUniqueID,             ExifTagInfo.ImageUniqueID },
            // { CameraOwnerName,           ExifTagInfo.CameraOwnerName },
            // { CameraSerialNumber,        ExifTagInfo.CameraSerialNumber },
            // { LensSpecification,         ExifTagInfo.LensSpecification },
            // { LensMake,                  ExifTagInfo.LensMake },
            // { LensModel,                 ExifTagInfo.LensModel },
            // { LensSerialNumber,          ExifTagInfo.LensSerialNumber },
            // { GDALMetadata,              ExifTagInfo.GDALMetadata },
            // { GDALNoData,                ExifTagInfo.GDALNoData },
            // { PrintImageMatching,        ExifTagInfo.PrintImageMatching },
            // { DNGVersion,                ExifTagInfo.DNGVersion },
            // { DNGBackwardVersion,        ExifTagInfo.DNGBackwardVersion },
            // { UniqueCameraModel,         ExifTagInfo.UniqueCameraModel },
            // { LocalizedCameraModel,      ExifTagInfo.LocalizedCameraModel },
            // { CFAPlaneColor,             ExifTagInfo.CFAPlaneColor },
            // { CFALayout,                 ExifTagInfo.CFALayout },
            // { LinearizationTable,        ExifTagInfo.LinearizationTable },
            // { BlackLevelRepeatDim,       ExifTagInfo.BlackLevelRepeatDim },
            // { BlackLevel,                ExifTagInfo.BlackLevel }

        };

        public static bool TryGet(ExifTag tag, out ExifTagInfo info)
        {
            return TagsToTypes.TryGetValue(tag, out info);
        }

        public static ExifTagInfo Get(ExifTag tag)
        {
            TagsToTypes.TryGetValue(tag, out var info);

            return info;
        }
    }
}

// http://www.exif.org/Exif2-2.PDF
