﻿namespace Carbon.Media
{
    public interface IMediaSource : ISize
    {
        string Key { get; }

        // change to: long Id
    }
}