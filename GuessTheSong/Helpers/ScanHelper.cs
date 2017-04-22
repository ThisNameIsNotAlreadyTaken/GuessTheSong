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

        private static List<Song> GetSongFromDirectory(string directoryPath, SongFileParseOptions parseOptions)
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
                                var info = name.Split(new[] {parseOptions.Delimeter}, StringSplitOptions.None);

                                string artistName = null;
                                string songName = null;
                                string priceString = null;

                                if (info.Length > 0)
                                {
                                    switch (parseOptions.FirstPart)
                                    {
                                        case SongFilePart.Artist:
                                            artistName = info[0];
                                            break;
                                        case SongFilePart.Name:
                                            songName = info[0];
                                            break;
                                        case SongFilePart.Price:
                                            priceString = info[0];
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }

                                if (info.Length > 1)
                                {
                                    switch (parseOptions.SecondPart)
                                    {
                                        case SongFilePart.Artist:
                                            artistName = info[1];
                                            break;
                                        case SongFilePart.Name:
                                            songName = info[1];
                                            break;
                                        case SongFilePart.Price:
                                            priceString = info[1];
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }

                                if (info.Length > 2)
                                {
                                    switch (parseOptions.ThirdPart)
                                    {
                                        case SongFilePart.Artist:
                                            artistName = info[2];
                                            break;
                                        case SongFilePart.Name:
                                            songName = info[2];
                                            break;
                                        case SongFilePart.Price:
                                            priceString = info[2];
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }

                                int price;

                                var isPriceComverted = int.TryParse(priceString, out price);

                                if (artistName == null && songName == null)
                                {
                                    throw new Exception("Could not found artist and song names in file");
                                }

                                if (!isPriceComverted)
                                {
                                    throw new Exception("Could not found valuable price in file");
                                }

                                result.ArtistName = artistName?.Trim();
                                result.Name = songName?.Trim().Substring(0, info[2].LastIndexOf(".", StringComparison.Ordinal) - 1);
                                result.Price = price;
                            }
                            catch (Exception e)
                            {
                                result.File.ParsingException = e;
                            }

                            return result;
                        }).OrderBy(x => x.Price).ToList();
        }

        private static Category GetCategoryFromDirectory(string directoryPath, SongFileParseOptions parseOptions)
        {
            var category = new Category
            {
                Name = directoryPath.Substring(directoryPath.LastIndexOf("\\", StringComparison.Ordinal) + 1), Songs = GetSongFromDirectory(directoryPath, parseOptions)
            };

            category.Songs.ForEach(s => s.CategoryName = category.Name);

            return category;
        }

        private static GameRound GetGameRoundFromDirectory(string directoryPath, SongFileParseOptions parseOptions)
        {
            return new GameRound
            {
                Name = directoryPath.Substring(directoryPath.LastIndexOf("\\", StringComparison.Ordinal) + 1), Categories = Directory.GetDirectories(directoryPath).ToList().Select(x => GetCategoryFromDirectory(x, parseOptions)).ToList()
            };
        }

        public static GameData GetGameDataFromDirectory(string directoryPath, SongFileParseOptions parseOptions, bool isMultipleRoundGame)
        {
            var topDirectories = Directory.GetDirectories(directoryPath).ToList();
            GameData gameData;


            if (isMultipleRoundGame)
            {
                gameData = new GameData
                {
                    Rounds = topDirectories.Select(x => GetGameRoundFromDirectory(x, parseOptions)).ToList()
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
                            Name = "Round", Categories = topDirectories.Select(x => GetCategoryFromDirectory(x, parseOptions)).ToList()
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
                                    warningNotes.Add($"File name {s.File.FullPath} is incorrect. Parsing Exception: {s.File.ParsingException.Message}");
                            });
                    });
            });

            return warningNotes;
        }
    }
}