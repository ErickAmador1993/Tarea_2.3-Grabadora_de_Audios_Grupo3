using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace XFAudioRecorderApp.Models
{
    public class AudioModel
    {

        [PrimaryKey, AutoIncrement]
        public int AudioId { get; set; }

        [MaxLength(200)]
        public string AudioUrl { get; set; }

        [MaxLength(80)]
        public string AudioDescripcion { get; set; }

    }
}
