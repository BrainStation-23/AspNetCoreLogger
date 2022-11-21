using System;

namespace WebApp.Logger.Test.Samples.NestedObject
{
    public class MyClass
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public MyAddress Address { get; set; }
        public MyProfile Profile { get; set; }
    }

    public class MyAddress
    {
        public long Id { get; set; }
        public string HouseNo { get; set; }
        public string RoadNo { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public MyCity City { get; set; }
    }

    public class MyCity
    {
        public long Id { get; set; }
        public string CityName { get; set; }
    }

    public class MyProfile
    {
        public long Id { get; set; }
        public string Fullname { get; set; }
        public string AboutMe { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? JobDate { get; set; }
    }
}
