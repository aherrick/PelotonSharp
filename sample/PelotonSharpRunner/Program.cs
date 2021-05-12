using Microsoft.Extensions.Configuration;
using PelotonSharp;
using PelotonSharpRunner.Helpers;
using System;
using System.Threading.Tasks;

namespace PelotonSharpRunner
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json").Build();

            var pelotonService = new PelotonService();

            var auth = await pelotonService.AuthenticateAsync(configuration["username_or_email"], configuration["password"]);

            var workoutList = await pelotonService.GetWorkoutListAsync(auth);

            foreach (var workout in workoutList)
            {
                var startTime = AppHelper.UnixTimeStampToDateTime(workout.start_time);
                var endTime = AppHelper.UnixTimeStampToDateTime(workout.end_time);

                Console.WriteLine($"{startTime} - {endTime} - {workout.ride.title}");
            }

            Console.ReadLine();
        }
    }
}