﻿namespace DotnetExam.Services.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}