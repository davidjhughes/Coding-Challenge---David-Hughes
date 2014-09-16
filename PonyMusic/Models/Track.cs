using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;


namespace PonyMusic.Models
{
    public class Track
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public double Timestamp { get; set; }
        public Artist Artist { get; set; }
        public Download Download { get; set; }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }



        public static async Task<IEnumerable<Models.Track>> GetAllFromURL()
        {

            using (var webClient = new WebClient())
            {
                Uri eqAddress = new Uri("https://eqbeats.org/tracks/all/json");
                string json = await webClient.DownloadStringTaskAsync(eqAddress);

                IEnumerable<Models.Track> tracks = JsonConvert.DeserializeObject<IEnumerable<Models.Track>>(json);
                foreach (var track in tracks)
                {
                    Debug.WriteLine(track.Download.Art);
                }


                return tracks;
            }
        }

        public static IEnumerable<Track> GetAllFromURLSync()
        {
            using (var webClient = new WebClient())
            {
                Uri eqAddress = new Uri("https://eqbeats.org/tracks/all/json");
                string json = webClient.DownloadString(eqAddress);

                IEnumerable<Track> tracks = JsonConvert.DeserializeObject<IEnumerable<Models.Track>>(json);


                return tracks;
            }


        }
    }
}