# Scribbly — .NET 8 Not Alma Uygulaması (MVC)

Scribbly; kullanıcıların notlarını oluşturup düzenleyebildiği, etiketleyebildiği ve arayabildiği modern bir **ASP.NET Core 8 MVC** uygulamasıdır. Kimlik doğrulama için **ASP.NET Core Identity**, veri erişimi için **Entity Framework Core** kullanır. UI tarafı Razor **Views** ile oluşturulmuş, doğrulamalar **DataAnnotations** ve özel validasyon öznitelikleriyle desteklenmiştir.

Canlı adres: https://scribbly.mkadirgulgun.com.tr/  
Test hesabı: **test@mkadirgulgun.com** / **123Test..**  (yalnızca deneme amaçlıdır ve periyodik olarak sıfırlanabilir)

## İçindekiler
- Özellikler
- Mimari ve Teknolojiler
- Proje Yapısı
- Hızlı Başlangıç
- Ortam Değişkenleri (appsettings.json)
- Veritabanı Göçleri (Migrations)
- Önemli Konular
- Yol Haritası
- Katkı ve Geliştirme
- Lisans

## Özellikler
- 👤 **Kimlik Doğrulama (ASP.NET Core Identity)**: kayıt, giriş, yetkilendirme.
- 📝 **Not Yönetimi**: oluştur, düzenle, sil; detay sayfası; listeleme.
- 🏷️ **Etiketleme**: notları etiketlerle ilişkilendir; etikete göre filtrele.
- 🔎 **Arama ve Filtreler**: başlık/içerik ve etiket bazlı arama (UI üzerinden).
- 🌐 **Yerelleştirme**: `Localizations` altında kaynak dosyalarıyla çok dillilik altyapısı.
- ✅ **Doğrulama**: `CustomValidations` ile ilave model doğrulamaları.
- 🧱 **Katmanlı Yapı**: Controller–Service–Data/EF–Model/DTO–ViewModel ayrımı.
- 🗂️ **Statik Dosyalar**: `wwwroot` altında stil/js/ikonlar.

> Not: Bu özet, repodaki klasör yapısı ve canlı sitede görülen akışa göre hazırlanmıştır. Uç noktalar ve ekranlar “Views/Controllers” içinde bulunur.

## Mimari ve Teknolojiler
- **ASP.NET Core 8 (MVC & Razor Views)**
- **Entity Framework Core** (Code‑First, Migrations)
- **ASP.NET Core Identity** (cookie tabanlı auth)
- **ViewModel / DTO** yaklaşımı
- **DataAnnotations & CustomValidations**
- **Options pattern** (`OptionsModels`)
- **Static files** (`wwwroot`)
- **Yerelleştirme** (`Localizations` klasörü)

## Proje Yapısı
Sık kullanılan klasörler:
- `Controllers/` — MVC controller’lar (Not, Etiket, Hesap vb.).
- `Services/` — İş mantığı/servisler (ör. not ve etiket işlemleri).
- `Data/` — `DbContext` ve EF Core yapılandırmaları.
- `Models/` — Varlık (entity) sınıfları.
- `ViewModels/` — Form ve görünüm modelleri.
- `CustomValidations/` — Özel doğrulama öznitelikleri.
- `Localizations/` — Kaynak dosyaları (çok dillilik).
- `OptionsModels/` — `IOptions<T>` ile kullanılan ayar sınıfları.
- `Views/` — Razor .cshtml sayfaları.
- `wwwroot/` — CSS/JS/görseller.
- `Migrations/` — EF Core göç dosyaları.
- `Program.cs`, `appsettings*.json`, `NoteTakingApp.sln`, `NoteTakingApp.csproj`.

## Hızlı Başlangıç
Önkoşullar:
- **.NET SDK 8.x**
- **SQL Server** (veya uyumlu bir sağlayıcı; varsayılan örnek SQL Server içindir)

Adımlar:
1) Depoyu klonlayın  
   `git clone https://github.com/mkadirgulgun/scribbly.git`  
   `cd scribbly`

2) Veritabanı bağlantınızı `appsettings.json` içinde ayarlayın (aşağıdaki örnek).

3) Bağımlılıkları derleyin ve veritabanını oluşturun:  
   `dotnet restore`  
   `dotnet build`  
   `dotnet ef database update`

4) Uygulamayı başlatın:  
   `dotnet run`  
   Çalışınca terminalde verilen URL’den giriş yapın (ör. `http://localhost:*****`).

## Ortam Değişkenleri (appsettings.json örneği)
Aşağıdaki örnek değerleri kendi ortamınıza göre güncelleyin:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ScribblyDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

> Üretimde güvenli bir bağlantı dizesi ve gizli anahtarlar için **User Secrets** veya ortam değişkenlerini tercih edin.

## Veritabanı Göçleri (Migrations)
Yeni bir tablo/alan ekleyince:
- Yeni migration oluşturun:  
  `dotnet ef migrations add <Migration_Adi>`
- Veritabanına uygulayın:  
  `dotnet ef database update`
- Geri almak için:  
  `dotnet ef database update <OncekiMigration>`

## Önemli Konular
- **Not–Etiket İlişkisi**: Notlar etiketlerle ilişkilendirilebilir (çok‑tan‑çoğa). UI üzerinden not eklerken/ düzenlerken etiket seçimi/oluşturma akışları mevcuttur.
- **Kimlik Doğrulama**: Kayıt/giriş/çıkış akışları Identity ile sağlanır. Gerekirse şifre kuralları ve lockout politikaları `Program.cs`/`IdentityOptions` üzerinden sıkılaştırılabilir.
- **Doğrulamalar**: Form girdileri için hem `DataAnnotations` hem de `CustomValidations` kullanılır (örn. minimum uzunluk, benzersizlik vb.).
- **Yerelleştirme**: `Localizations` klasöründeki kaynaklarla metinler yerelleştirilebilir. Varsayılan kültür ve desteklenen diller `Program.cs` içinde ayarlanır.
- **Statik İçerik**: `wwwroot` altında CSS/JS. Yayında ters proxy (IIS/Nginx) ile statik içerik önbelleklemesi önerilir.
- **Güvenlik**: `CookiePolicy`, `AntiForgeryToken` (formlarda), `Authorization` nitelikleri ile sayfa/aksiyon koruması.

## Yol Haritası
- [ ] Etiket/çoklu seçim deneyimini güçlendirme (autocomplete).
- [ ] Not içi tam metin arama (opsiyonel: EF + FTS).
- [ ] Soft‑delete/arşiv akışı ve geri yükleme ekranları.
- [ ] Gelişmiş sıralama/paginasyon.
- [ ] Dosya ekleri (görsel/doküman) desteği.
- [ ] CI (GitHub Actions) ve örnek birim testleri.
