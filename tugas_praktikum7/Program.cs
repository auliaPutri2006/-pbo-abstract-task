using System;

public interface Ikemampuan
{
    string Nama { get; }
    int CoolDown { get; }
    void Digunakan(Robot target);
    bool DapatDigunakan();
}

public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }
    protected int LimitEnergi { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
        LimitEnergi = energi;
    }

    public virtual void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        damage = Math.Max(damage, 0);
        target.Energi -= damage;
        Console.WriteLine($"{Nama} melakukan serangan kepada {target.Nama} yang menyebabkan kerusakan sebesar {damage}");
    }

    public abstract void GunakanKemampuan(Ikemampuan kemampuan, Robot target);

    public void CetakInformasi()
    {
        Console.WriteLine($"\nDetail Robot: memiliki Nama = {Nama} dengan Energi = {Energi}, Armor = {Armor} dan Serangan = {Serangan}");
    }

    public void EnergiPulih(int jumlah)
    {
        Energi = Math.Min(Energi + jumlah, LimitEnergi);
        Console.WriteLine($"pemain {Nama} memulihkan energi sebesar {jumlah}");
    }
}

public class BossRobot : Robot
{
    public BossRobot(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan) { }

    public override void Serang(Robot target)
    {
        int damage = (Serangan * 3) - target.Armor;
        damage = Math.Max(damage, 0);
        target.Energi -= damage;
        Console.WriteLine($"pemain {Nama} menyerang {target.Nama} dengan kerusakan sebesar {damage}");
    }

    public override void GunakanKemampuan(Ikemampuan kemampuan, Robot target)
    {
        Console.WriteLine($"{Nama} sedang menggunakan kemampuan {kemampuan.Nama}");
        kemampuan.Digunakan(target);
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah mengalami kekalahan!");
    }
}

public class Perbaikan : Ikemampuan
{
    public string Nama => "Perbaikan";
    public int CoolDown { get; private set; } = 3;
    private int cooldownTimer = 0;

    public void Digunakan(Robot target)
    {
        if (DapatDigunakan())
        {
            target.EnergiPulih(20);
            cooldownTimer = CoolDown;
            Console.WriteLine($"pemain {target.Nama} mengalami pemulihan energi sebesar 20 poin");
        }
        else
        {
            Console.WriteLine("Kemampuan sedang cooldown");
        }
    }

    public bool DapatDigunakan() => cooldownTimer == 0;
    public void KurangiCooldown() => cooldownTimer = Math.Max(0, cooldownTimer - 1);
}

public class seranganlistrik : Ikemampuan
{
    public string Nama => "serangan listrik (electric shock)";
    public int CoolDown { get; private set; } = 2;
    private int cooldownTimer = 0;

    public void Digunakan(Robot target)
    {
        if (DapatDigunakan())
        {
            target.Energi -= 20;
            cooldownTimer = CoolDown;
            Console.WriteLine($"pemain {target.Nama} mengalami serangan listrik yang dapat mempengaruhi gerekan mereka sehingga kehilangan 20 poin kekuatan ");
        }
        else { Console.WriteLine("Sedang mengalami cooldown"); }


    }
    public bool DapatDigunakan() => cooldownTimer == 0;
    public void KurangiCooldown() => cooldownTimer = Math.Max(0, cooldownTimer - 1);
}

    public class seranganplasma : Ikemampuan
    {
        public string Nama => "serangan plasma (plasma cannon)";
        public int CoolDown { get; private set; } = 4;
        private int cooldownTimer = 0;

        public void Digunakan(Robot target)
        {
            if (DapatDigunakan())
            {
                target.Energi -= 20;
                cooldownTimer = CoolDown;
                Console.WriteLine($"pemain {target.Nama} mengalami serangan listrik hingga menembus armor lawan dan menyebabkan kehilangan 20 poin kekuatan ");
            }
            else { Console.WriteLine("sedang mengalami cooldown"); }

        }
        public bool DapatDigunakan() => cooldownTimer == 0;
        public void KurangiCooldown() => cooldownTimer = Math.Max(0, cooldownTimer - 1);
    }

    public class pertahananSuper : Ikemampuan
    {
        public string Nama => "pertahanan super (super shield)";
        public int CoolDown { get; private set; } = 3;
        private int cooldownTimer = 0;

        public void Digunakan(Robot target)
        {
            if (DapatDigunakan())
            {
                target.Armor += 15;
                cooldownTimer = CoolDown;
                Console.WriteLine($"pemain {target.Nama} meningkatkan kekuatan armor sementara sehingga kekuatan armor meningkat 15 poin");
            }
            else
            {
                Console.WriteLine("sedang mengalami cooldown");
            }
        }
        public bool DapatDigunakan() => cooldownTimer == 0;
        public void KurangiCooldown() => cooldownTimer = Math.Max(0, cooldownTimer - 1);
    }

    public class RobotMini : Robot
    {
        private Ikemampuan kemampuanPerbaikan = new Perbaikan();
        public RobotMini(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan) { }

        public override void GunakanKemampuan(Ikemampuan kemampuan, Robot target)
        {
            if (kemampuan == null) kemampuan = kemampuanPerbaikan;
            Console.WriteLine($"pemain{Nama} sedang  menggunakan kemampuan {kemampuan.Nama}");
            kemampuan.Digunakan(target);
        }
    }
    public class robotlistrik : Robot
    {
        private Ikemampuan kemampuanlistrik = new seranganlistrik();
        public robotlistrik(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan) { }

        public override void GunakanKemampuan(Ikemampuan kemampuan, Robot target)
        {
            if (kemampuan == null) kemampuan = kemampuanlistrik;
            Console.WriteLine($"pemain{Nama} sedang menggunakan kemampuan {kemampuan.Nama}");
            kemampuan.Digunakan(target);
        }
    }
    public class robotplasma : Robot
    {
        private Ikemampuan kemampuanplasma = new seranganplasma();
        public robotplasma(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan) { }

        public override void GunakanKemampuan(Ikemampuan kemampuan, Robot target)
        {
            if (kemampuan == null) kemampuan = kemampuanplasma;
            Console.WriteLine($"pemain {Nama} sedang menggunakan kemampuan {kemampuan.Nama}");
            kemampuan.Digunakan(target);
        }
    }

    public class robotbertahan : Robot
    {
        private Ikemampuan kemampuanbertahan = new pertahananSuper();
        public robotbertahan(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan) { }
        public override void GunakanKemampuan(Ikemampuan kemampuan, Robot target)
        {
            if (kemampuan == null) kemampuan = kemampuanbertahan;
            Console.WriteLine($"pemain {Nama} sedang menggunakan kemampuan {kemampuan.Nama}");
            kemampuan.Digunakan(target);
        }
    }

    public class Program
    {
        public static void Main()
        {
            Robot robotMini1 = new RobotMini("Robot Mini Ungu", 150, 7, 10);
            Robot robotMini2 = new RobotMini("Robot Mini Merah", 100, 5, 20);
            Robot robotlistrik1 = new robotlistrik("Robot listrik hitam", 150, 15, 30);
            Robot robotlistrik2 = new robotlistrik("Robot listrik pink", 140, 20, 30);
            Robot robotplasma1 = new robotplasma("Robot plasma kuning", 150, 10, 15);
            Robot robotplasma2 = new robotplasma("Robot plasma Abu-Abu", 90, 6, 17);
            Robot robotbertahan1 = new robotbertahan("Robot bertahan jingga", 145, 16, 24);
            Robot robotbertahan2 = new robotbertahan("Robot bertahan biru", 100, 10, 36);

            BossRobot bosRobot1 = new BossRobot("Boss Tertinggi", 400, 25, 35);

            robotMini1.CetakInformasi();
            robotMini2.CetakInformasi();
            robotlistrik1.CetakInformasi();
            robotlistrik2.CetakInformasi();
            robotplasma1.CetakInformasi();
            robotplasma2.CetakInformasi();
            robotbertahan1.CetakInformasi();
            robotbertahan2.CetakInformasi() ;
            bosRobot1.CetakInformasi();

            Robot[] robotList = { robotMini1, robotMini2, robotlistrik1, robotlistrik2, robotplasma1, robotplasma2, robotbertahan1, robotbertahan2 };

            for (int round = 1; round <= 5; round++)
            {
                Console.WriteLine($"\nPutaran ke-{round}");

                foreach (Robot robot in robotList)
                {
                    robot.GunakanKemampuan(null, bosRobot1);
                }

                Random rand = new Random();
                int targetInd = rand.Next(robotList.Length);
                bosRobot1.Serang(robotList[targetInd]);

                foreach (Robot robot in robotList)
                {
                    robot.EnergiPulih(5);
                }

                if (bosRobot1.Energi <= 0)
                {
                    bosRobot1.Mati();
                    break;
                }
            }

            Console.WriteLine("Informasi detail robot:");
            foreach (Robot robot in robotList)
            {
                robot.CetakInformasi();
            }
            bosRobot1.CetakInformasi();
        }
    }
