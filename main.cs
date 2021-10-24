/*http://www.infojegyzet.hu/vizsgafeladatok/okj-programozas/szoftverfejleszto-190514/
fuvar.csv:
taxi_id;indulas;idotartam;tavolsag;viteldij;borravalo;fizetes_modja
5240;2016-12-15 23:45:00;900;2,5;10,75;2,45;bankkártya
*/
using System;       // Console
using System.IO;    // StreamReader
using System.Text;  // Encoding
using System.Collections.Generic; // List<>, Dictionary<>
using System.Linq;  // from where group by orderby select

class Fuvar{
    public int      taxi_id         {get; set; }
    public string   indulas         {get; set; }
    public int      idotartam       {get; set; }
    public float    tavolsag        {get; set; }
    public float    viteldij        {get; set; }
    public float    borravalo       {get; set; }
    public string   fizetes_modja   {get; set; }

    public Fuvar(string sor){
        var s = sor.Trim().Replace(',','.').Split(';');
        this.taxi_id       = int.Parse(s[0]); 
        this.indulas       = s[1]; 
        this.idotartam     = int.Parse(s[2]);
        this.tavolsag      = float.Parse(s[3]);
        this.viteldij      = float.Parse(s[4]);
        this.borravalo     = float.Parse(s[5]);
        this.fizetes_modja = s[6];
    }
}

class Program {
  public static void Main (string[] args) {
    // 2. feladat 
    var f = new StreamReader("fuvar.csv");
    var elsosor = f.ReadLine();    
    var lapok = new List<Fuvar>(); 
    while(!f.EndOfStream){
        var  lap = new Fuvar( f.ReadLine() );    
        lapok.Add(lap);
    }
    f.Close();

    // 3. feladat: {} fuvar
    Console.WriteLine($"3. feladat: {lapok.Count} fuvar");
    
    // 4. feladat: a 6185 -ös fuvarjainak száma és bevétele
    var bevetelek = ( 
        from lap in lapok 
        where lap.taxi_id == 6185 
        select (lap.viteldij + lap.borravalo) 
        );

    Console.WriteLine($"4. feladat: {bevetelek.Count()} fuvar alatt: {bevetelek.Sum()}$");

    // 5. feladat: Fizetési statisztika
    var query = ( from lap in lapok group lap by lap.fizetes_modja );
    
    Console.WriteLine(    $"5. feladat:");
    foreach( var q in query ){
        Console.WriteLine($"        {q.Key}: {q.Count()} fuvar");
    }
    
    // 6. feladat Összes km-ek száma
    var tavolsagok = ( from lap in lapok select lap.tavolsag * 1.6);
    Console.WriteLine($"6. feladat: {tavolsagok.Sum():.##}km");

    // 7. feladat: Leghosszabb idejű fuvar:
    var leghosszab_fuvar = (from lap in lapok orderby lap.idotartam select lap).Last();
    Console.WriteLine($"7. feladat:");
    Console.WriteLine($"        Fuvar hossza: {leghosszab_fuvar.idotartam} másodperc:");
    Console.WriteLine($"        Taxi azonosító: {leghosszab_fuvar.taxi_id} ");
    Console.WriteLine($"        Megtett távolság: {leghosszab_fuvar.tavolsag:.#} km");
    Console.WriteLine($"        Viteldíj: {leghosszab_fuvar.viteldij}$");
    
    // 8. feladat hibak.txt létrehozása
    var hibak = (
        from lap in lapok
        where (lap.idotartam > 0) & (lap.viteldij > 0) & (lap.tavolsag == 0)
        orderby lap.indulas
        select lap
    ); 
    var f_hibak = new StreamWriter("Hibak.txt");
    f_hibak.WriteLine(elsosor);
    foreach(var i in hibak){
        var sor = $"{i.taxi_id};{i.indulas};{i.idotartam};{i.tavolsag};{i.viteldij};{i.borravalo};{i.fizetes_modja}";
        f_hibak.WriteLine(sor.Replace('.',','));
    }
    f_hibak.Close();
//-------------------------------------      
  }
}