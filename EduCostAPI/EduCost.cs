﻿using System;
using System.IO;
using System.Net;

namespace EduCostAPI
{
    public class EduCost
    {
        public double getCost(string CollegeName, bool RoomAndBoard)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://raw.githubusercontent.com/tylerdng/EduCostAPI/master/EduCostAPI/Resources/college_costs.csv");
            StreamReader reader = new StreamReader(stream);
            String content = reader.ReadToEnd();
            StreamReader csvReader = new StreamReader(File.OpenRead("C:Resources/college_costs.csv"));
            while (!csvReader.EndOfStream)
            {
                string[] data = csvReader.ReadLine().Split(",");
                if (CollegeName.ToLower().Equals(data[0].ToLower()))
                {
                    double cost = Convert.ToDouble(data[1]);
                    if(RoomAndBoard) cost += Convert.ToDouble(data[3]);
                    return cost;
                }
            }

            return -1;
        }
    }
}
