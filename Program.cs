using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NReco.VideoConverter;
using System.IO;
using System.Reflection;

namespace ThumbnailExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "Extract":

                    Extract(int.Parse(args[1])).Wait();

                    break;
                case "PreviewMP4":

                    PreviewMP4(args[1], args[2]).Wait();

                    break;
                case "PreviewMP3":

                    PreviewMP3(args[1], args[2]).Wait();

                    break;

                case "MPG":
                    MPG(args[1]).Wait();
                    break;
                default:
                    Console.WriteLine("wrong command. List of available commands:\nExtract PreviewMP4 MPG");
                    break;
            }

        }

        static async Task Extract(int FrameTime)
        {

            try
            {
                var converter = new FFMpegConverter();

                var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var files = Directory.GetFiles(currentPath, "*.mp4");
                int sum=0;
                if (files.Any())
                {
                    foreach (var item in files)
                    {
                        var stream = new FileStream(currentPath + "\\" + Path.GetFileNameWithoutExtension(item) + ".jpg", FileMode.Create);

                        converter.GetVideoThumbnail(item, stream, FrameTime);

                        await stream.FlushAsync();

                        stream.Close();
                        sum++;
                        Console.WriteLine("thumbnail for "+Path.GetFileNameWithoutExtension(item)+" was created.");
                    }
                }
                else
                {
                    //no mp4 files found
                    Console.WriteLine("No mp4 video found.");
                }

                Console.WriteLine("Done:) Total of "+sum+" thumbnails were created.");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        static async Task PreviewMP4(string Start, string End)
        {

            try
            {
                var converter = new FFMpegConverter();

                var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var files = Directory.GetFiles(currentPath, "*.mp4");

                if (files.Any())
                {
                    foreach (var item in files)
                    {
                        var output = currentPath + "\\" + Path.GetFileNameWithoutExtension(item) + "-pr.mp4";
                        Console.WriteLine("-ss " + Start + " -i " + item + " -to " + End + " -c copy -y " + output);
                        converter.Invoke("-ss " + Start + " -i " + item + " -to " + End + " -c copy -y " + output);

                    }
                }
                else
                {
                    //no mp4 files found
                    Console.WriteLine("No mp4 video found.");
                }
                Console.WriteLine("Done:)");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        static async Task PreviewMP3(string Start, string End)
        {

            try
            {
                var converter = new FFMpegConverter();

                var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var files = Directory.GetFiles(currentPath, "*.mp3");

                if (files.Any())
                {
                    foreach (var item in files)
                    {
                        var output = currentPath + "\\" + Path.GetFileNameWithoutExtension(item) + "-pr.mp3";

                        converter.Invoke("-ss " + Start + " -i " + item + " -t " + End + " -y " + output);

                    }
                }
                else
                {
                    //no mp4 files found
                    Console.WriteLine("No mp3 video found.");
                }
                Console.WriteLine("Done:)");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        static async Task MPG(string directoryPath)
        {

            try
            {
                var converter = new FFMpegConverter();

                var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var files = Directory.GetFiles(currentPath, "*.mp4");

                if (files.Any())
                {
                    foreach (var item in files)
                    {
                        var output = directoryPath + "\\" + Path.GetFileNameWithoutExtension(item) + ".mpg";

                        converter.Invoke("-i " + item + " -f lavfi -i anullsrc -ac 2 -qscale:v 1 -shortest -max_muxing_queue_size 800 -r 25 -y " + output);

                    }
                }
                else
                {
                    //no mp4 files found
                    Console.WriteLine("No mp4 video found.");
                }

                Console.WriteLine("Done:)");
                Console.ReadLine();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
