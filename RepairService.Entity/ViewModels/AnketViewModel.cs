using RepairService.Entity.Kisi;
using RepairService.Entity.Models;
using RepairService.Entity.Models.Cihaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService.Entity.ViewModels
{
    public class AnketViewModel
    {
        //Anket Sorular Cevaplar //Servis Musteri
        public ServisKaydi Servis { get; set; }
        public Musteri Musteri { get; set; }
        public string AnketBaslik { get; set; }
        public int AnketID { get; set; }
        public List<AnketSoru> Sorular { get; set; } = new List<AnketSoru>();
        public List<AnketSorusununCevap> Cevaplar { get; set; } = new List<AnketSorusununCevap>();
    }

    //public class Question
    //{
    //    public int ID { set; get; }
    //    public string QuestionText { set; get; }
    //    public List<Answer> Answers { set; get; }
    //    public string SelectedAnswer { set; get; }
    //    public Question()
    //    {
    //        Answers = new List<Answer>();
    //    }
    //}
    //public class Answer
    //{
    //    public int ID { set; get; }
    //    public string AnswerText { set; get; }
    //}
    //public class Evaluation
    //{
    //    public List<Question> Questions { set; get; }
    //    public Evaluation()
    //    {
    //        Questions = new List<Question>();
    //    }
    //}

}
