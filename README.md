# Silk Library

[![NuGet version (PelotonSharp)](https://img.shields.io/nuget/v/PelotonSharp.svg?style=flat-square)](https://www.nuget.org/packages/PelotonSharp/)
![Publish](https://github.com/aherrick/PelotonSharp/workflows/Publish/badge.svg?branch=main)

```
Install-Package PelotonSharp
```

# PelotonSharp



## Basic Usage

```csharp
            // Authenticate.
            // you can pull user/pass from a configuration file (recommended) or hardcode (not recommended)

            var auth = await PelotonService.AuthenticateAsync(configuration["username_or_email"], configuration["password"]);

            // Get all Workouts.
            var workoutList = await PelotonService.GetWorkoutListAsync(auth);
```

 Easily access your Peloton workout data!

![Drag Racing](https://raw.githubusercontent.com/aherrick/PelotonSharp/main/assets/PelotonSharpIcon.png)