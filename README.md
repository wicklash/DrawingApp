# DrawingApp

Bu proje, C# ve Windows Forms kullanılarak geliştirilmiş,basit bir çizim uygulamasıdır. Kullanıcılar uygulama üzerinde çeşitli geometrik şekiller  çizebilir, bu şekilleri seçip taşıyabilir, renklerini değiştirebilir ve çizim tahtasını bir dosyaya kaydedip sonradan tekrar yükleyebilirler.

### 1. (Abstraction)
Tüm çizilebilir şekillerin ortak özelliklerini ve davranışlarını barındıran **`Shape`** isimli soyut (abstract) bir sınıf yer almaktadır.

*   `Shape` sınıfı; `x`, `y`, `width`, `height`, `color` gibi temel özellikleri barındırır.
*   Bu sınıf içerisinde gövdesi olmayan `Draw(Graphics g)` ve `IsHit(int mouseX, int mouseY)` adında **abstract** metotlar tanımlanmıştır. Bu sayede `Shape` sınıfından türeyen tüm alt sınıflar bu metotların içini kendileri bir şekilde doldurmak zorundadır.
*   Ayrıca nesne seçildiğinde gölge efekti yaratmasına yarayan sanal **`virtual`** bir `Shadow(Graphics g)` metodu bulunur. Bu metot alt sınıflar tarafından doğrudan kullanılabilir veya override edilebilir.

### 2. (Polymorphism)
`Rectangle`, `Circle`, `Triangle` ve `Hexagon` sınıfları `Shape` sınıfından inherit ederler. 

*   Her bir şekil sınıfı, `Shape` sınıfından miras aldığı `Draw` ve `IsHit` metotlarını kendi geometrik özelliklerine göre override farklı şekillerde uygular.
    *   Örneğin; `Rectangle.cs` içindeki `Draw` metodu `g.FillRectangle` fonksiyonunu çağırırken, `Circle.cs` içindeki `Draw` metodu `g.FillEllipse` fonskiyonunu çağırır. 
    *   `IsHit` metodu ise kullanıcının fareyle tıkladığı noktanın o anki şeklin sınırları içerisinde olup olmadığını kontrol eder.
*   `Form1.cs` içerisinde şekiller özel bir tip yerine genel bir `List<Shape>` listesinde tutulur. Ekrana çizdirme  esnasında sadece `shape.Draw(e.Graphics)` çağrılır.

### 3. (Encapsulation)
Sınıflar içerisindeki verilerin ve yardımcı fonksiyonların sadece o sınıf içerisinden erişilebilir olması sağlanmıştır.
*   `Hexagon` ve `Triangle` sınıflarında köşe noktalarını hesaplamak (ör: `CreatePoints()`) ve alan kontrolü yapmak için kullanılan (ör: `IsPointInTriangle()`) bazı metotlar ve değişkenler (`private Point[] points`) sadece o sınıfı ilgilendirdiği için **private** olarak işaretlenerek dışarıdan erişime kapatılmıştır.


### 4. Static Kullanımı
*   **`FileManager` Sınıfı:** Dosya işlemlerini (kaydetme ve yükleme) gerçekleştirmek için oluşturulmuştur ve **`static`** olarak tanımlanmıştır. Bu sınıftan nesne üretilmeden doğrudan `FileManager.savePanel` ve `FileManager.LoadPanel` şeklinde çağrılırlar.
*  Dosyadan kayıtlı şekiller okunurken string bir metin ("Circle", "Rectangle" vs.) okunur. `CreateShapeInstance` adındaki özel bir metot bu okunan string bilgiye göre uygun sınıftan bir nesne (`new Circle()`, `new Rectangle()` vb.) üretir.
