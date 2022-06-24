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
            //ProjectData(googleApps);
            //DivideData(googleApps);
            //OrderData(googleApps);
            //DataSetOperation(googleApps);
            DataVerification(googleApps);
        }

        static void DataVerification(IEnumerable<GoogleApp> googleApps)
        {
            var allOperatorResult = googleApps.Where(app => app.Category == Category.WEATHER)
                .All(a => a.Reviews > 20);

            Console.WriteLine($"allOperatorResult: {allOperatorResult}");

            var anyOperatorResult = googleApps.Where(app => app.Category == Category.WEATHER)
                .Any(a => a.Reviews > 2_000_000);

            Console.WriteLine($"allOperatorResult: {anyOperatorResult}");
        }

        static void DataSetOperation(IEnumerable<GoogleApp> googleApps)
        {
            var paidAppsCategories = googleApps.Where(app => app.Type == Type.Paid)
                .Select(a => a.Category)
                .Distinct();

            //Console.WriteLine($"Paid apps categories {string.Join(", ", paidAppsCategories)}");

            var setA = googleApps.Where(app => app.Rating > 4.7 && app.Type == Type.Paid && app.Reviews > 1000);

            var setB = googleApps.Where(app => app.Name.Contains("Pro") && app.Rating > 4.6 && app.Reviews > 10000);

            //Display(setA);
            //Console.WriteLine("***********");
            //Display(setB);

            //var appsUnion = setA.Union(setB);
            //Console.WriteLine("Apps Union");
            //Display(appsUnion);

            var appsIntersect = setA.Intersect(setB);
            Console.WriteLine("Apps Intersect");
            Display(appsIntersect);

            Console.WriteLine("******************");

            var appExcept = setA.Except(setB);
            Console.WriteLine("Apps except");
            Display(appExcept);
        }

        static void OrderData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedBeautyApps = googleApps.Where(app => app.Rating > 4.4 && app.Category == Category.BEAUTY);
            Display(highRatedBeautyApps);

            Console.WriteLine("break");

            //var sortedResults = highRatedBeautyApps.OrderBy(app => app.Rating);
            var sortedResults = highRatedBeautyApps
                .OrderByDescending(app => app.Rating)
                .ThenBy(app => app.Name)
                .Take(5);

            Display(sortedResults);
        }

        static void DivideData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
            Console.WriteLine("high rated");
            Display(highRatedBeautyApps);

            //var first5HighRatedBeautyApps = new List<GoogleApp>();

            //foreach (GoogleApp app in highRatedBeautyApps)
            //{
            //    first5HighRatedBeautyApps.Add(app);
            //    if (first5HighRatedBeautyApps.Count == 5)
            //    {
            //        break;
            //    }
            //}

            //var first5HighRatedBeautyApps = highRatedBeautyApps.Take(5);
            //var first5HighRatedBeautyApps = highRatedBeautyApps.TakeLast(5);
            //var first5HighRatedBeautyApps = highRatedBeautyApps.TakeWhile(app => app.Reviews > 1000);

            //var skippedResults = highRatedBeautyApps.Skip(5);
            var skippedResults = highRatedBeautyApps.SkipWhile(app => app.Reviews > 1000);
            Console.WriteLine("skipped");
            Display(skippedResults);
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

            var annonymousDtos = highRatedBeautyApps.Select(app => new
            {
                Reviews = app.Reviews,
                Name = app.Name,
                Category = app.Category
            });

            foreach (var dto in dtos)
            {
                Console.WriteLine($"{dto.Name}: {dto.Reviews}");
            }

            foreach (var dto in annonymousDtos)
            {
                Console.WriteLine($"{dto.Name}: {dto.Reviews}");
            }

            Console.WriteLine(String.Join(", ", highRatedBeautyAppsNames));

            //var genres = highRatedBeautyApps.Select(app => app.Genres);
            var genres = highRatedBeautyApps.SelectMany(app => app.Genres);
            Console.WriteLine(string.Join(": ", genres));
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


