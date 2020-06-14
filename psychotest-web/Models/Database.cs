using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Extensions;
using Firebase.Database.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;

namespace psychotest.Model
{
    public static class Database
    {
        static string authSecret = "xxxxx";
        static string basePath = "https://psychotest-c14cf.firebaseio.com/";

        public static async Task<ProbeResult> GetProbe(string probeID)
        {
            var client = new FirebaseClient(basePath, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authSecret)
            });

            try
            {
                var probeCollection = await client.Child("badania").
                    OrderByKey().
                    EqualTo(probeID).OnceAsync<ProbeResult>();

                var probe = probeCollection.First().Object;
                Console.WriteLine("Znaleziono rekord");
                Console.WriteLine("Done: " + probe.Done.ToString());
                return probe;
            }
            catch (FirebaseException)
            {
                throw new WebException();
            }
            catch(Exception e)
            {
                Console.WriteLine("Nie znaleziono rekordu o kluczu: " + probeID);
                Console.WriteLine(e.GetType());
                throw new ArgumentOutOfRangeException("Nie znaleziono ID");
            }
        }



        public static async Task PostProbe(ProbeResult probe)
        {
            var client = new FirebaseClient(basePath, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authSecret)
            });
            try
            {
                await client.Child("badania/" + probe.ID).PutAsync(probe);
            }
            catch(FirebaseException)
            {
               // MessageBox.Show("Problem z wysyłaniem wyniku. Proszę sprawdzić połączenie internetowe i wykonać test ponownie");
            }
        }
    }
}
