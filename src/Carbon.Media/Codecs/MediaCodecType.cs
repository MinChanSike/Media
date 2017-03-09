﻿namespace Carbon.Media
{
	public enum MediaCodecType
	{
        Unknown = 0,
        
        // -------------------- Audio Codecs (200-299) ---------------------------------------
        
        Aac      = 200,
        Ac3      = 205,
        Aiff     = 210,
        Alac     = 215, // Apple Lossless
        Flac     = 220, // Free Lossless Audio Codec
        Mp3      = 230, // MPEG-1 Audio Layer 3
        Opus     = 240,
        Pcm      = 250,
        Speex    = 255,
        Vorbis   = 260,
        Wav      = 270,
        Wma      = 280, //  Windows Media Audio
        
        // -------------------- Image Codecs (400-499) ---------------------------------------
        
        Bmp      = 401,
        Bpg      = 402,
        Dng      = 408, // Digital Negative
        Gif      = 410,
        Heif     = 420, // High Efficiency Image File Format
        Ico      = 425,
        Jp2      = 430, // JPEG2000
        Jpeg     = 431,
        Jxr      = 432, // JPEG-XR
        Png      = 450,
        Psd      = 460,
        Svg      = 460,
        Tiff     = 470,
        WebP     = 480,
        
        // TODO: Raw formats: https://en.wikipedia.org/wiki/Raw_image_format
        
        // -------------------- Video Codecs (900-999)  ---------------------------------------
        
        Blackbird = 901,
        Cinepak   = 903,
        Dirac     = 905,
        DivX      = 906,
        
        H261      = 921,
        H262      = 922,
        H263      = 923, // MPEG-3
        H264      = 924, // MPEG-4 (mp4)
        H265      = 925, // HEVC
        
        Huffyuv   = 920,
        
        Lagarith  = 925,
        
        Sorenson3 = 940,
        
        Theora    = 945, // Derived from On2's VP3 Codec.
        
        Vc1       = 950, // Windows Media Video V9
        
        Vp3       = 953,
        Vp4       = 954,
        Vp5       = 955,
        Vp6       = 956, // TrueMotion VP6
        Vp6E      = 957,
        Vp6S      = 959,
        Vp7       = 960,
        Vp8       = 961, // libvpx (used in WebM)
        Vp9       = 962,
        
        Wmv7      = 967,
        Wmv8      = 968,
        Wmv9      = 969
    }
}
