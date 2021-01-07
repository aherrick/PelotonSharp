namespace PelotonSharp.Models
{
    public class UserWorkoutDetails
    {
        public string workout_type { get; set; }
        public float total_work { get; set; }
        public bool is_total_work_personal_record { get; set; }
        public string device_type { get; set; }
        public int total_leaderboard_users { get; set; }
        public string timezone { get; set; }
        public int leaderboard_rank { get; set; }
        public int device_time_created_at { get; set; }
        public string id { get; set; }
        public object fitbit_id { get; set; }
        public string peloton_id { get; set; }
        public string user_id { get; set; }
        public object title { get; set; }
        public bool has_leaderboard_metrics { get; set; }
        public bool has_pedaling_metrics { get; set; }
        public string platform { get; set; }
        public string metrics_type { get; set; }
        public string fitness_discipline { get; set; }
        public string status { get; set; }
        public string device_type_display_name { get; set; }
        public int start_time { get; set; }
        public string name { get; set; }
        public string strava_id { get; set; }
        public int created { get; set; }
        public int created_at { get; set; }
        public Ftp_Info ftp_info { get; set; }
        public int end_time { get; set; }
        public Ride ride { get; set; }
    }

    public class Ftp_Info
    {
        public string ftp_source { get; set; }
        public int ftp { get; set; }
        public object ftp_workout_id { get; set; }
    }
}