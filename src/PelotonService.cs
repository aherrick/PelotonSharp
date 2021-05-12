using Newtonsoft.Json;
using PelotonRunner.Models;
using PelotonSharp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PelotonSharp
{
    public class PelotonService
    {
        private readonly HttpClient _client;

        public PelotonService(HttpClient httpClient = null)
        {
            _client = httpClient;

            if (_client == null)
            {
                _client = new HttpClient();
            }

            if (!_client.DefaultRequestHeaders.TryGetValues("User-Agent", out _))
            {
                _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            }
        }

        public async Task<AuthResponse> AuthenticateAsync(string user, string password)
        {
            var info = new { password, username_or_email = user };

            var httpContent = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");

            var respMsg = await _client.PostAsync("https://api.onepeloton.com/auth/login", httpContent);
            var respMsgJson = await respMsg.Content.ReadAsStringAsync();

            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(respMsgJson);

            return authResponse;
        }

        public async Task<List<RideDatum>> GetWorkoutListAsync(AuthResponse auth)
        {
            var rideDataList = new List<RideDatum>();

            _client.DefaultRequestHeaders.TryAddWithoutValidation("cookie", $"peloton_session_id={auth.session_id}");

            int pageNum = 0;
            while (true)
            {
                var workoutListRespJson = await _client.GetStringAsync($"https://api.onepeloton.com/api/user/{auth.user_id}/workouts?joins=ride&limit=20&page={pageNum}");
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

        public async Task<UserWorkoutDetails> GetWorkoutUserDetails(RideDatum ride)
        {
            var userDetailsJson = await _client.GetStringAsync($"https://api.onepeloton.com/api/workout/{ride.id}");

            var userDetails = JsonConvert.DeserializeObject<UserWorkoutDetails>(userDetailsJson);

            return userDetails;
        }

        public async Task<EventDetails> GetWorkoutEventDetails(RideDatum ride)
        {
            var userDetailsJson = await _client.GetStringAsync($"https://api.onepeloton.com/api/ride/{ride.ride.id}/details");

            var eventDetails = JsonConvert.DeserializeObject<EventDetails>(userDetailsJson);

            return eventDetails;
        }

        public async Task<WorkoutSessionMetrics> GetWorkoutMetricsAsync(RideDatum ride, int secondsPerObservation = 1)
        {
            var workoutSessionJson = await _client.GetStringAsync($"https://api.onepeloton.com/api/workout/{ride.id}/performance_graph?every_n={secondsPerObservation}");

            var workoutSessionMetrics = JsonConvert.DeserializeObject<WorkoutSessionMetrics>(workoutSessionJson);

            return workoutSessionMetrics;
        }

        private async Task Throttle()
        {
            await Task.Delay(1000);
        }
    }
}