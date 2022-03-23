using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using XFAudioRecorderApp.Models;

namespace XFAudioRecorderApp.Controllers
{
    public class AudioModelDB
    {
        readonly SQLiteAsyncConnection db;

        public AudioModelDB(string pathdb)
        {
            db = new SQLiteAsyncConnection(pathdb);
            db.CreateTableAsync<AudioModel>().Wait();
        }

        public Task<List<AudioModel>> ObtenerListaAudios()
        {
            return db.Table<AudioModel>().ToListAsync();
        }

        public Task<AudioModel> ObtenerAudio(int pcodigo)
        {
            return db.Table<AudioModel>()
                .Where(i => i.AudioId == pcodigo)
                .FirstOrDefaultAsync();
        }

        public Task<int> GrabarAudio(AudioModel audio)
        {
            if (audio.AudioId != 0)
            {
                return db.UpdateAsync(audio);
            }
            else
            {
                return db.InsertAsync(audio);
            }
        }

        public Task<int> EliminarAudio(AudioModel audio)
        {
            return db.DeleteAsync(audio);
        }


    }
}
