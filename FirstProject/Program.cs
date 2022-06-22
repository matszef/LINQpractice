using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

// ReSharper disable UseFormatSpecifierInInterpolation

namespace FirstProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvPath = @"C:\Users\MS_PC\source\repos\Data\googleplaystore1.csv";
            var googleApps = LoadGoogleAps(csvPath);

            //GetData(googleApps);
            ProjectData(googleApps);
            
        }

        static void ProjectData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
            var highRatedBeautyAppsNames = highRatedBeautyApps.Select(app => app.Name);

            //var highRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);

            //mapowanie
            var dtos = highRatedBeautyApps.Select(app => new GoogleAppDto()
            {
                Reviews = app.Reviews,
                Name = app.Name
            });

            foreach (var dto in dtos)
            {
                Console.WriteLine($"{dto.Name}: {dto.Reviews}");
            }

            Console.WriteLine(String.Join(", ", highRatedBeautyAppsNames));

            var genres = highRatedBeautyApps.SelectMany(app => app.Genres);
            Console.WriteLine(string.Join(": ", genres);
            
        }

        static void GetData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps.Where(app => app.Rating > 4.6);
            var highRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
            Display(highRatedBeautyApps);

            //var firsthighRatedBeautyApp = highRatedBeautyApps.First(app => app.Reviews < 300);
            //var firsthighRatedBeautyApp = highRatedBeautyApps.FirstOrDefault(app => app.Reviews < 50);
            //var firsthighRatedBeautyApp = highRatedBeautyApps.Single(app => app.Reviews < 200);
            //var firsthighRatedBeautyApp = highRatedBeautyApps.SingleOrDefault(app => app.Reviews < 50);
            //var firsthighRatedBeautyApp = highRatedBeautyApps.Last(app => app.Reviews < 200);
            var firsthighRatedBeautyApp = highRatedBeautyApps.LastOrDefault(app => app.Reviews < 50);
            Console.WriteLine("firsthighRatedBeautyApp");
            Console.WriteLine(firsthighRatedBeautyApp);
        }

        static void Display(IEnumerable<GoogleApp> googleApps)
        {
            foreach (var googleApp in googleApps)
            {
                Console.WriteLine(googleApp);
            }

        }
        static void Display(GoogleApp googleApp)
        {
            Console.WriteLine(googleApp);
        }

        static List<GoogleApp> LoadGoogleAps(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GoogleAppMap>();
                var records = csv.GetRecords<GoogleApp>().ToList();
                return records;
            }

        }

    }

    
}


