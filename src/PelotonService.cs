using Newtonsoft.Json;
using PelotonRunner.Models;
using PelotonSharp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PelotonSharp
{
    public static class PelotonService
    {
        public static async Task<AuthResponse> AuthenticateAsync(string user, string password)
        {
            var client = new ttpClient();

            var info = new { password, username_or_email = user };

            var httpContent = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");

            var respMsg = await client.PostAsync("https://api.onepeloton.com/auth/login", httpContent);
            var respMsgJson = await respMsg.Content.ReadAsStringAsync();

            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(respMsgJson);

            return authResponse;
        }

        public static async Task<List<RideDatum>> GetWorkoutListAsync(AuthResponse auth)
        {
            var client = new HttpClient();

            var rideDataList = new List<RideDatum>();

            client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", $"peloton_session_id={auth.session_id}");

            int pageNum = 0;
            while (true)
            {
                var workoutListRespJson = await client.GetStringAsync($"https://api.onepeloton.com/api/user/{auth.user_id}/workouts?joins=ride&limit=20&page={pageNum}");
                var workoutList = JsonConvert.DeserializeObject<WorkoutList>(workoutListRespJson);

                rideDataList.AddRange(workoutList.data);

                if (workoutList.show_next)
                {
                    pageNum++;
                    await Throttle();
                }
                else
                {
                    break;
                }
            }

            return rideDataList;
        }

        public static async Task<UserWorkoutDetails> GetWorkoutUserDetails(RideDatum ride)
        {
            var client = new HttpClient();

            var userDetailsJson = await client.GetStringAsync($"https://api.onepeloton.com/api/workout/{ride.id}");

            var userDetails = JsonConvert.DeserializeObject<UserWorkoutDetails>(userDetailsJson);

            return userDetails;
        }

        public static async Task<EventDetails> GetWorkoutEventDetails(RideDatum ride)
        {
            var client = new HttpClient();

            var userDetailsJson = await client.GetStringAsync($"https://api.onepeloton.com/api/ride/{ride.ride.id}/details");

            var eventDetails = JsonConvert.DeserializeObject<EventDetails>(userDetailsJson);

            return eventDetails;
        }

        public static async Task<WorkoutSessionMetrics> GetWorkoutMetricsAsync(RideDatum ride, int secondsPerObservation = 1)
        {
            var client = new HttpClient();

            var workoutSessionJson = await client.GetStringAsync($"https://api.onepeloton.com/api/workout/{ride.id}/performance_graph?every_n={secondsPerObservation}");

            var workoutSessionMetrics = JsonConvert.DeserializeObject<WorkoutSessionMetrics>(workoutSessionJson);

            return workoutSessionMetrics;
        }

        private static async Task Throttle()
        {
            await Task.Delay(1000);
        }
    }
}