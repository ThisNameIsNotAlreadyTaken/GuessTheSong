using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuessTheSong.Models;

namespace GuessTheSong.Helpers
{
    public static class ScanHelper
    {
        private static readonly List<string> ExtensionsList = new List<string> { ".mp3", ".wav", "wma" };

        private static List<Song> GetSongFromDirectory(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly).ToList()
                        .Where(x => ExtensionsList.Any(y => x.EndsWith(y, StringComparison.OrdinalIgnoreCase))).Select(f =>
                        {
                            var result = new Song
                            {
                                File = new SongFile
                                {
                                    FullPath = f
                                }
                            };

                            try
                            {
                                var name = f.Substring(f.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                                var info = name.Split(new[] {"--"}, StringSplitOptions.None);

                                var artistName = info.Length > 1 ? info[1].Trim() : null;
                                var songName = info.Length > 2
                                    ? info[2].Trim()
                                        .Substring(0, info[2].LastIndexOf(".", StringComparison.Ordinal) - 1)
                                    : null;

                                var price = 0;

                                if (info.Length > 0) int.TryParse(info[0].Trim(), out price);

                                result.ArtistName = artistName;
                                result.Name = songName;
                                result.Price = price;
                            }
                            catch (Exception e)
                            {
                                result.File.ParsingException = e;
                            }

                            return result;

                        }).OrderBy(x => x.Price).ToList();
        }

        private static Category GetCategoryFromDirectory(string directoryPath)
        {
            var category = new Category
            {
                Name = directoryPath.Substring(directoryPath.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                Songs = GetSongFromDirectory(directoryPath)
            };

            category.Songs.ForEach(s => s.CategoryName = category.Name);

            return category;
        }

        private static GameRound GetGameRoundFromDirectory(string directoryPath)
        {
            return new GameRound
            {
                Name = directoryPath.Substring(directoryPath.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                Categories = Directory.GetDirectories(directoryPath).ToList().Select(GetCategoryFromDirectory).ToList()
            };
        }

        public static GameData GetGameDataFromDirectory(string directoryPath, bool isMultipleRoundGame)
        {
            var topDirectories = Directory.GetDirectories(directoryPath).ToList();
            GameData gameData;


            if (isMultipleRoundGame)
            {
                gameData = new GameData
                {
                    Rounds = topDirectories.Select(GetGameRoundFromDirectory).ToList()
                };
            }
            else
            {
                gameData = new GameData
                {
                    Rounds = new List<GameRound>
                    {
                        new GameRound
                        {
                            Name = "Round",
                            Categories = topDirectories.Select(GetCategoryFromDirectory).ToList()
                        }
                    }
                };
            }

            gameData.WarningNotes = CheckGameData(gameData);

            return gameData;
        }

        private const string CorrectTreeSuggestion = " Ensure that the directory tree was configured correctly.";

        public static List<string> CheckGameData(GameData gameData)
        {
            var warningNotes = new List<string>();

            if (gameData.Rounds.IsNullOrEmpty()) warningNotes.Add($"Game has no round. {CorrectTreeSuggestion}");

            gameData.Rounds.ForEach(r =>
            {
                if (r.Categories.IsNullOrEmpty())
                    warningNotes.Add($"Round {r.Name} has no categories. {CorrectTreeSuggestion}");
                else
                    r.Categories.ForEach(c =>
                    {
                        if (c.Songs.IsNullOrEmpty())
                            warningNotes.Add($"Category {c.Name} has no songs. {CorrectTreeSuggestion}");
                        else
                            c.Songs.ForEach(s =>
                            {
                                if (s.File.ParsingException != null)
                                    warningNotes.Add(
                                        $"File name {s.File.FullPath} is incorrect. Parsing Exception: {s.File.ParsingException.Message}");
                            });
                    });
            });

            return warningNotes;
        }
    }
}
