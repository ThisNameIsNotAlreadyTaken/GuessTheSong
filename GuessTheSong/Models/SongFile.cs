using System;

namespace GuessTheSong.Models
{
    public class SongFile
    {
        public string FullPath { get; set; }

        public string FileName => FullPath.Substring(FullPath.LastIndexOf("\\", StringComparison.Ordinal) + 1);

        public string FilePath => FullPath.Substring(0, FullPath.LastIndexOf("\\", StringComparison.Ordinal));

        public string FileNameWithoutExtension
            => FileName.Substring(0, FileName.LastIndexOf(".", StringComparison.Ordinal));
    }
}
