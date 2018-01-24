using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using PagedList;

namespace NCCFOhaukwu.Web.Models
{
    public class ListOfThings
    {
        public static List<SelectListItem> GenderList()
        {
            var genderList = new List<SelectListItem>
            {
                new SelectListItem {Text = "Male", Value = "Male"},
                new SelectListItem {Text = "Female", Value = "Female"}
            };

            return genderList;
        }

        public static List<SelectListItem> Meridian()
        {
            var meridianList = new List<SelectListItem>
            {
                new SelectListItem {Text = "am", Value = "am"},
                new SelectListItem {Text = "pm", Value = "pm"}
            };

            return meridianList;
        }

        public static List<SelectListItem> MonthList()
        {
            var monthList = new List<SelectListItem>
            {
                new SelectListItem {Text = "January", Value = "January"},
                new SelectListItem {Text = "February", Value = "February"},
                new SelectListItem {Text = "March", Value = "March"},
                new SelectListItem {Text = "April", Value = "April"},
                new SelectListItem {Text = "May", Value = "May"},
                new SelectListItem {Text = "June", Value = "June"},
                new SelectListItem {Text = "July", Value = "July"},
                new SelectListItem {Text = "August", Value = "August"},
                new SelectListItem {Text = "September", Value = "September"},
                new SelectListItem {Text = "October", Value = "October"},
                new SelectListItem {Text = "November", Value = "November"},
                new SelectListItem {Text = "December", Value = "December"}
            };

            return monthList;
        }

        public static int EventMonth(string month)
        {
            var monthInt = 0;
            switch (month)
            {
                case "January":
                    monthInt = 1;
                    break;
                case "February":
                    monthInt = 2;
                    break;
                case "March":
                    monthInt = 3;
                    break;
                case "April":
                    monthInt = 4;
                    break;
                case "May":
                    monthInt = 5;
                    break;
                case "June":
                    monthInt = 6;
                    break;
                case "July":
                    monthInt = 7;
                    break;
                case "August":
                    monthInt = 8;
                    break;
                case "September":
                    monthInt = 9;
                    break;
                case "October":
                    monthInt = 10;
                    break;
                case "November":
                    monthInt = 11;
                    break;
                case "December":
                    monthInt = 12;
                    break;
            }

            return monthInt;
        }

        public static List<SelectListItem> MaritalStatusList()
        {
            var maritalStatusList = new List<SelectListItem>
            {
                new SelectListItem {Text = "Single", Value = "Single"},
                new SelectListItem {Text = "Married", Value = "Married"},
                new SelectListItem {Text = "Divorced", Value = "Divorced"},
                new SelectListItem {Text = "Engaged", Value = "Engaged"}
            };

            return maritalStatusList;
        }

        public static List<SelectListItem> PpaTownList()
        {
            var ppaTownList = new List<SelectListItem>
            {
                new SelectListItem {Text = "Ezzangbo", Value = "Ezzangbo"},
                new SelectListItem {Text = "Ngbo", Value = "Ngbo"},
                new SelectListItem {Text = "Effium", Value = "Effium"}
            };

            return ppaTownList;
        }

        public static List<SelectListItem> ZoneSubZoneList()
        {
            var zoneSubZoneList = new List<SelectListItem>
            {
                new SelectListItem {Text = "Ohaukwu", Value = "Ohaukwu"},
                new SelectListItem {Text = "Ezzangbo", Value = "Ezzangbo"},
                new SelectListItem {Text = "Ngbo", Value = "Ngbo"},
                new SelectListItem {Text = "Effium", Value = "Effium"}
            };

            return zoneSubZoneList;
        }

        public static List<SelectListItem> ResourcesList()
        {
            var resourceList = new List<SelectListItem>
            {
                new SelectListItem {Text = "Album", Value = "Album"},
                new SelectListItem {Text = "AlbumPhoto", Value = "AlbumPhoto"},
                new SelectListItem {Text = "Batch", Value = "Batch"},
                new SelectListItem {Text = "CorpMember", Value = "CorpMember"},
                new SelectListItem {Text = "Event", Value = "Event"},
                new SelectListItem {Text = "News", Value = "News"},
                new SelectListItem {Text = "Portfolio", Value = "Portfolio"},
                new SelectListItem {Text = "Project", Value = "Project"},
                new SelectListItem {Text = "ProjectPicture", Value = "ProjectPicture"},
                new SelectListItem {Text = "ServiceYear", Value = "ServiceYear"},
                new SelectListItem {Text = "Users", Value = "Users"},
                new SelectListItem {Text = "Roles", Value = "Roles"},
                new SelectListItem {Text = "Manage", Value = "Manage"},
                new SelectListItem {Text = "ResourceOperation", Value = "ResourceOperation"},
                new SelectListItem {Text = "Give", Value = "Give"},
                new SelectListItem {Text = "Carousel", Value = "Carousel"},
                new SelectListItem {Text = "WhoWeAre", Value = "WhoWeAre"},
                new SelectListItem {Text = "NewHere", Value = "NewHere"},
                new SelectListItem {Text = "Article", Value = "Article"},
                new SelectListItem {Text = "Sermon", Value = "Sermon"}
            };

            return resourceList;
        }

        public static List<SelectListItem> OperationsList()
        {
            var operationList = new List<SelectListItem>
            {
                new SelectListItem {Text = "Create", Value = "1"},
                new SelectListItem {Text = "Read", Value = "2"},
                new SelectListItem {Text = "Update", Value = "4"},
                new SelectListItem {Text = "Delete", Value = "8"},
                new SelectListItem {Text = "Execute", Value = "16"}
            };

            return operationList;
        }
    }

    public class CorpMembers
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Other Name")]
        public string OtherName { get; set; }

        [Required]
        [StringLength(11)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Day { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public string Year { get; set; }

        [Display(Name = "State of Origin")]
        public string StateOfOrigin { get; set; }

        [Display(Name = "LGA")]
        public string Lga { get; set; }

        [Required]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Personal Information")]
        [AllowHtml]
        public string PersonalInfo { get; set; }

        [Display(Name = "Academic Information")]
        [AllowHtml]
        public string AcademicInfo { get; set; }

        [Display(Name = "NCCF Information")]
        [AllowHtml]
        public string NccfInfo { get; set; }

        [Display(Name = "Sub Zone")]
        public string PpaTown { get; set; }

        public bool IsGeneral { get; set; }
        public bool IsLeader { get; set; }

        [Display(Name = "Service Year")]
        [Required]
        public int YearId { get; set; }

        [Display(Name = "Batch")]
        [Required]
        public int BatchId { get; set; }

        [Display(Name = "Position(s)")]
        public string Positions { get; set; }

        public string PositionIds { get; set; }
        public byte[] MiniImage { get; set; }
        public string MiniContentType { get; set; }
        public byte[] MajorImage { get; set; }
        public string MajorContentType { get; set; }
        public string[] SelectedPositions { get; set; }

        public List<SelectListItem> GenderList
        {
            get { return ListOfThings.GenderList(); }
        }

        public List<SelectListItem> MonthList
        {
            get { return ListOfThings.MonthList(); }
        }

        public List<SelectListItem> MaritalStatusList
        {
            get { return ListOfThings.MaritalStatusList(); }
        }

        public List<SelectListItem> PpaTownList
        {
            get { return ListOfThings.PpaTownList(); }
        }
    }

    public class AlbumPhotos
    {
        public int Id { get; set; }

        [Display(Name = "Album")]
        [Required]
        public int AlbumId { get; set; }

        [Display(Name = "Service Year")]
        [Required]
        public int YearId { get; set; }

        [Display(Name = "Batch")]
        [Required]
        public int BatchId { get; set; }

        public string ContentType { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }

    public class PicsDesc
    {
        public HttpPostedFileBase PictureUpload { get; set; }
        public string Description { get; set; }
    }

    public class Albums
    {
        public int Id { get; set; }

        [Display(Name = "Album Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Zone/Sub Zone")]
        [Required]
        public string SubZone { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public List<SelectListItem> PpaTownList
        {
            get { return ListOfThings.ZoneSubZoneList(); }
        }
    }

    public class TheNews
    {
        public int Id { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public string Title { get; set; }
        public bool Display { get; set; }
        public string ContentType { get; set; }
        public byte[] Photo { get; set; }

        [Display(Name = "The News")]
        [AllowHtml]
        public string News { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        public string Username { get; set; }
    }

    public class Projects
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Sub Zone")]
        public string SubZone { get; set; }

        [Required]
        [AllowHtml]
        public string Milestone { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        public List<SelectListItem> PpaTownList
        {
            get { return ListOfThings.ZoneSubZoneList(); }
        }
    }

    public class WhoWeAreModel
    {
        public int Id { get; set; }

        [Display(Name = "Who We Are")]
        [Required]
        [AllowHtml]
        public string Whoweare { get; set; }

        [Display(Name = "Our History")]
        [Required]
        [AllowHtml]
        public string OurHistory { get; set; }

        [Display(Name = "Our Beliefs")]
        [Required]
        [AllowHtml]
        public string OurBeliefs { get; set; }

        [Display(Name = "Our Mission")]
        [Required]
        [AllowHtml]
        public string OurMission { get; set; }
    }

    public class GiveModel
    {
        public int Id { get; set; }

        [Required]
        [AllowHtml]
        public string Donate { get; set; }
    }

    public class NewHereModel
    {
        public int Id { get; set; }

        [Display(Name = "New to NCCF")]
        [Required]
        [AllowHtml]
        public string NewToNccf { get; set; }

        [Display(Name = "What to Expect")]
        [Required]
        [AllowHtml]
        public string WhatToExpect { get; set; }

        [Display(Name = "Family Song")]
        [Required]
        [AllowHtml]
        public string FamilySong { get; set; }

        [Display(Name = "Service Times and Locations")]
        [Required]
        [AllowHtml]
        public string ServiceTimesandLocations { get; set; }
    }

    public class Events
    {
        public int id { get; set; }

        [Display(Name = "Event Title")]
        public string title { get; set; }

        [Display(Name = "Day")]
        public string StartDay { get; set; }

        [Display(Name = "Month")]
        public string StartMonth { get; set; }

        [Display(Name = "Year")]
        public string StartYear { get; set; }

        [Display(Name = "Scheduled date")]
        public string start { get; set; }
        public string end { get; set; }

        [Display(Name = "Start time")]
        public string StartTime { get; set; }

        public string StartTimeMeridian { get; set; }

        [Display(Name = "End time")]
        public string EndTime { get; set; }

        public string EndTimeMeridian { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public string Venue { get; set; }

        [Display(Name = "Venue Full Address")]
        public string VenueAddress { get; set; }

        public string url { get; set; }

        public List<SelectListItem> MonthList
        {
            get { return ListOfThings.MonthList(); }
        }

        public List<SelectListItem> Meridian
        {
            get { return ListOfThings.Meridian(); }
        }
    }

    public class Home
    {
        public List<Carousel> Carousels { get; set; }
        public List<News> News { get; set; }
        public List<Event> Events { get; set; }
        public Sermon Sermon { get; set; }
        public List<Articles> Articles { get; set; }
    }

    public class SermonModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Scriptures { get; set; }

        [AllowHtml]
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
    public class ArticlesModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Article { get; set; }
        public bool Display { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        public string Username { get; set; }
    }

    public class MiniImageConpresser
    {
        public static byte[] CompressImage(Stream img)
        {
            byte[] finalImage = null;

            var image = new Bitmap(img);

            var codecs = ImageCodecInfo.GetImageEncoders();
            var jpgEncoder = codecs.FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

            if (jpgEncoder != null)
            {
                var encoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);

                long quality = 40;

                var encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                var ms = new MemoryStream();
                image.Save(ms, jpgEncoder, encoderParameters);

                var reader = new BinaryReader(ms);
                ms.Position = 0;
                finalImage = reader.ReadBytes(Convert.ToInt32(ms.Length));
                ms.Flush();
                ms.Close();
            }
            return finalImage;
        }
    }

    public class ImageConpresser
    {
        public static byte[] CompressImage(Stream img)
        {
            byte[] finalImage = null;
            
            var image = new Bitmap(img);

            var originalWidth = image.Width;
            var originalHeight = image.Height;

            var ratio = 1200 / (float)originalWidth;

            var newWidth = (int)(originalWidth * ratio);
            var newHeight = (int)(originalHeight * ratio);
                
            var newImage = new Bitmap(image, newWidth, newHeight);

            var codecs = ImageCodecInfo.GetImageEncoders();
            var jpgEncoder = codecs.FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

            if (jpgEncoder != null)
            {
                var encoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);

                long quality = 50;

                var encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                var ms = new MemoryStream();
                newImage.Save(ms, jpgEncoder, encoderParameters);

                var reader = new BinaryReader(ms);
                ms.Position = 0;
                finalImage = reader.ReadBytes(Convert.ToInt32(ms.Length));
                ms.Flush();
                ms.Close();
            }
            return finalImage;
        }
    }

    public class CarouselImageConpresser
    {
        public static byte[] CompressImage(Stream img)
        {
            byte[] finalImage = null;

            var image = new Bitmap(img);

            var originalWidth = image.Width;
            var originalHeight = image.Height;

            var ratioX = 1500 / (float)originalWidth;
            var ratioY = 900 / (float)originalWidth;

            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(originalWidth * ratio);
            var newHeight = (int)(originalHeight * ratio);

            var newImage = new Bitmap(image, newWidth, newHeight);

            //if (image.Size.Width != 1500)
            //{
            //    return finalImage;
            //}

            //if (image.Size.Height != 900)
            //{
            //    return finalImage;
            //}

            var codecs = ImageCodecInfo.GetImageEncoders();
            var jpgEncoder = codecs.FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

            if (jpgEncoder != null)
            {
                var encoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);

                long quality = 50;

                var encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                var ms = new MemoryStream();
                newImage.Save(ms, jpgEncoder, encoderParameters);

                var reader = new BinaryReader(ms);
                ms.Position = 0;
                finalImage = reader.ReadBytes(Convert.ToInt32(ms.Length));
                ms.Flush();
                ms.Close();
            }
            return finalImage;
        }
    }
}