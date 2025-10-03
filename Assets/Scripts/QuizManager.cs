using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;
    public Text questionText;
    public Button[] answerButtons;
    private System.Action<bool> callback;

    private string[][] questionsIPA = {
        new string[] { "Apa yang merupakan sumber utama energi untuk kehidupan di Bumi?", "Matahari", "Air", "Angin", "Bensin" },
        new string[] { "Tanaman memerlukan cahaya matahari untuk proses?", "Fotosintesis", "Transpirasi", "Respirasi", "Germinasi" },
        new string[] { "Hewan yang berkembang biak dengan cara bertelur disebut?", "Ovipar", "Vivipar", "Ovovivipar", "Herbivora"},
        new string[] { "Bagian dari tumbuhan yang berfungsi menyerap air dan mineral dari tanah adalah?", "Akar", "Daun", "Batang", "Bunga" },
        new string[] { "Planet yang terdekat dengan Matahari adalah?", "Merkurius", "Bumi", "Mars", "Jupyter" },
        new string[] { "Kekuatan yang menyebabkan benda jatuh ke bawah adalah?", "Gaya gravitasi", "Gaya gesek", "Gaya magnet", "Gaya dorong" },
        new string[] { "Bagian tubuh manusia yang berfungsi untuk memompa darah ke seluruh tubuh adalah?", "Jantung", "Hati", "Paru-paru", "Ginjal" },
        new string[] { "Benda langit yang mengelilingi planet disebut?", "Satelit", "Komet", "Asteroid", "Bintang" },
        new string[] { "Bagian tumbuhan yang berfungsi sebagai tempat terjadinya fotosintesis adalah?", "Daun", "Akar", "Batang", "Ranting" },
        new string[] { "Zat yang menyebabkan warna hijau pada daun disebut?", "Klorofil", "Karotenoid", "Flavonoid", "Antosianin" },
        new string[] { "Hewan yang bernafas dengan insang saat masih muda dan dengan paru-paru saat dewasa adalah?", "Katak", "Ular", "Penyu", "Kadal" },
        new string[] { "Alat untuk mengukur suhu disebut?", "Termometer", "Barometer", "Hygrometer", "Anemometer" },
        new string[] { "Sistem organ manusia yang berfungsi untuk menghancurkan makanan menjadi zat yang lebih sederhana adalah?", "Sistem pencernaan", "Sistem pernapasan", "Sistem peredaran darah", "Sistem ekskresi" },
        new string[] { "Alat kelamin jantan pada bunga disebut?", "Benang Sari", "Putik", "Kelopak", "Mahkota" },
        new string[] { "Hewan yang memakan tumbuhan dan daging disebut?", "Omnivora", "Karnivora", "Herbivora", "Insektivora" },
        new string[] { "Bagian tubuh manusia yang berfungsi sebagai pusat pengendali adalah?", "Otak", "Hati", "Jantung", "Paru-paru" },
        new string[] { "Proses pertukaran gas oksigen dan karbon dioksida pada manusia terjadi pada?", "Paru-paru", "Hidung", "Kulit", "Jantung" },
        new string[] { "Bagian dari mata yang berfungsi untuk melihat warna adalah?", "Retina", "Kornea", "Lensa", "Iris" },
        new string[] { "Bumi berputar pada porosnya selama?", "24 jam", "12 jam", "20 jam", "16 jam" },
        new string[] { "Hewan berikut yang termasuk ke dalam kelompok serangga adalah?", "Kupu-kupu", "Kuda", "Kadal", "Kura-kura" },

    };

    private string[][] questionsIPS = {
        new string[] { "Siapa presiden pertama Indonesia?", "Soekarno", "Soeharto", "Habibie", "Gus Dur" },
        new string[] { "Siapa presiden Indonesia saat ini?", "Jokowi", "Soeharto", "Habibie", "Megawati" },
        new string[] { "Ibukota Indonesia adalah?", "Jakarta", "Bandung", "Aceh", "Kalimantan" },
        new string[] { "Mata uang negara Indonesia adalah?", "Rupiah", "Ringgit", "Baht", "Dollar" },
        new string[] { "Semboyan negara Indonesia adalah?", "Bhinneka Tunggal Ika", "Merdeka atau Mati", "Satu Nusa Satu Bangsa", "Semangat Kebangsaan" },
        new string[] { "Bendera negara Indonesia berwarna?", "Merah dan Putih", "Merah dan Biru", "Biru dan Putih", "Hijau dan Kuning" },
        new string[] { "Pancasila adalah dasar negara Indonesia yang terdiri dari?", "5 sila", "2 sila", "4 sila", "6 sila" },
        new string[] { "Hari Kemerdekaan Indonesia diperingati setiap tanggal?", "17 Agustus", "18 Agustus", "17 Oktober", "17 Desember" },
        new string[] { "Lambang negara Indonesia adalah?", "Garuda Pancasila", "Merah Putih", "Keris", "Bunga Melati" },
        new string[] { "Lagu kebangsaan Indonesia adalah?", "Indonesia Raya", "Indonesia Pusaka", "Garuda Pancasila", "Tanah Airku" },
        new string[] { "Tugu Monas terletak di kota?", "Jakarta", "Yogyakarta", "Bandung", "Depok" },
        new string[] { "Pada lambang Garuda Pancasila, pita yang dicengkeram oleh Garuda bertuliskan?", "Bhinneka Tunggal Ika", "Indonesia Raya", "Pancasila", "Merdeka" },
        new string[] { "Siapa yang memproklamasikan kemerdekaan Indonesia?", "Soekarno & Moh. Hatta", "Soeharto", "Habibie", "Gus Dur" },
        new string[] { "Sungai apa yang terpanjang di Indonesia?", "Kapuas", "Mahakam", "Musi", "Citarum" },
        new string[] { "Siapa yang menjahit bendera pusaka Indonesia?", "Fatmawati", "Kartini", "Cut Meutia", "Soekarno" },
        new string[] { "Siapa yang dikenal sebagai 'Bapak Pendidikan Indonesia'?", "Ki Hajar Dewantara", "Soekarno", "Kartini", "Moh. Hatta" },
        new string[] { "Bentuk pemerintahan negara Indonesia adalah?", "Republik", "Kerajaan", "Kesultanan", "Federasi" },
        new string[] { "Hari Pendidikan Nasional diperingati setiap tanggal?", "2 Mei", "17 Agustus", "1 Oktober", "28 Oktober" },
        new string[] { "Gunung apa yang dinobatkan sebagai gunung tertinggi di Indonesia?", "Jayawijaya", "Kerinci", "Semeru", "Rinjani" },

    };

    private string[][] questionsMath = {
        new string[] { "Berapa hasil dari 5 + 3?", "8", "9", "7", "6" },
        new string[] { "Berapakah hasil dari 9 × 3?", "27", "9", "12", "6" },
        new string[] { "Jika Andi memiliki 12 apel dan membagikannya kepada 4 temannya, berapa banyak apel yang didapatkan setiap teman?", "3", "2", "4", "6" },
        new string[] { "Rina memiliki 3 apel, 2 jeruk, dan 4 pisang. Buah apa yang paling banyak dimiliki Rina?", "Pisang", "Jeruk", "Apel", "Sama banyak" },
        new string[] { "Budi membeli permen seharga Rp5.000 dan coklat seharga Rp3.000. Berapa total uang yang dibayarkan Budi?", "Rp8.000", "Rp5.000", "Rp10.000", "Rp6.000" },
        new string[] { "Berapa hasil dari 18 - 9?", "9", "11", "5", "6" },
        new string[] { "Berapakah hasil dari 8 ÷ 2?", "4", "16", "10", "6" },
        new string[] { "Berapakah hasil dari 4 × 4?", "16", "20", "8", "1" },
        new string[] { "Berapa hasil dari 6 + 7?", "13", "1", "7", "6" },
        new string[] { "Berapa hasil dari 20 - 8?", "12", "28", "7", "6" },
        new string[] { "Berapa hasil dari 18 ÷ 6?", "3", "24", "12", "6" },
        new string[] { "Berapa hasil dari 16 - 7?", "9", "10", "7", "6" },
        new string[] { "Budi membeli 3 kotak cokelat. Setiap kotak berisi 5 cokelat. Berapa jumlah cokelat yang dibeli Budi?", "15", "9", "7", "6" },
        new string[] { "Sebuah kelas memiliki 28 siswa. Jika siswa tersebut dibagi ke dalam 4 kelompok, berapa siswa dalam setiap kelompok?", "7", "9", "Salah semua", "6" },
        new string[] { "Sebuah keranjang berisi 36 apel. Jika 9 apel rusak, berapa apel yang masih baik?", "27", "29", "72", "25" },
        new string[] { "Jika 6 buku harganya Rp 30.000, berapa harga satu buku?", "Rp 5.000", "Rp 6.000", "Rp 7.000", "Rp 8.000" },
        new string[] { "Berapa hasil dari 50 ÷ 5?", "10", "55", "45", "6" },
        new string[] { "Berapa hasil dari 8 × 4?", "32", "23", "30", "12" },
        new string[] { "Jumlah sudut dalam sebuah segitiga adalah?", "180 derajat", "90 derajat", "360 derajat", "120 derajat" },
        new string[] { "Berapa hasil dari 70 + 3?", "73", "93", "72", "67" },

    };

    private string[][] currentQuestions;
    private int correctAnswerIndex;
    private string correctAnswer;

    public void ShowQuiz(string theme, System.Action<bool> resultCallback)
    {
        callback = resultCallback;
        quizPanel.SetActive(true);
        SetQuestionsBasedOnTheme(theme);
        DisplayQuestion();
    }

    private void SetQuestionsBasedOnTheme(string theme)
    {
        switch (theme)
        {
            case "IPA":
                currentQuestions = questionsIPA;
                break;
            case "IPS":
                currentQuestions = questionsIPS;
                break;
            case "Matematika":
                currentQuestions = questionsMath;
                break;
            case "Campuran":
                currentQuestions = questionsIPA.Concat(questionsIPS).Concat(questionsMath).ToArray();
                break;
        }
    }

    private void DisplayQuestion()
    {
        int questionIndex = Random.Range(0, currentQuestions.Length);
        string[] selectedQuestion = currentQuestions[questionIndex];

        questionText.text = selectedQuestion[0];
        correctAnswer = selectedQuestion[1];

        string[] shuffledAnswers = selectedQuestion.Skip(1).OrderBy(a => Random.value).ToArray();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = shuffledAnswers[i];
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => AnswerSelected(shuffledAnswers[index]));
        }
    }

    public void AnswerSelected(string selectedAnswer)
    {
        bool isCorrect = selectedAnswer == correctAnswer;
        quizPanel.SetActive(false);
        callback.Invoke(isCorrect);
    }

    public void HideQuizPanel()
    {
        quizPanel.SetActive(false);
    }
}