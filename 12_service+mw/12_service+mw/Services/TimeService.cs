﻿namespace _12_service_mw.Services;

public class TimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToString("HH:mm:ss");
}
