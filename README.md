
# Redis Cache Demo Projesi

Bu, .NET 9 uygulamasında Redis entegrasyonu ve önbellekleme tekniklerini göstermeyi amaçlayan basit bir demo projesidir. 
Proje, Redis'i dağıtılmış önbellekleme için kullanmanın yanı sıra temel veri tipleri, karmaşık tipler ve dosya önbellekleme gibi çeşitli önbellekleme tekniklerini de gösterir. 
Bu proje, Redis entegrasyonunu ve önbellekleme stratejilerini sergilemek için hazırlanmıştır ve gelecekteki projeler için referans olarak kullanılabilir.

## Proje Yapısı

- **RedisCacheService**: Redis ile etkileşime giren servis. Aşağıdaki işlemleri gerçekleştirebilir:
    - Temel veri tiplerini (örneğin, string) önbelleğe almak.
    - Karmaşık nesneleri (örneğin, C# nesneleri) önbelleğe almak.
    - Dosyaları (örneğin, byte dizileri) önbelleğe almak.

- **RedisStreamWorker**: Redis stream kullanarak verileri işleyen bir arka plan servisi. Bu servis, verileri alır ve yeniden deneme işlemi gerçekleştirir.

## Gereksinimler

- **Redis**: Redis'in bilgisayarınızda yüklü olması veya bir bağlantı dizesi ile erişilebilir olması gereklidir.
- .NET 9 SDK.

## Kurulum

### 1. Depoyu Klonlayın:

```bash
git clone https://github.com/voyvodka/RedisCacheDemo.git
cd redis-cache-demo
```

### 2. Bağımlılıkları Yükleyin:

Proje bağımlılıklarını yüklemek için aşağıdaki komutu çalıştırın.

```bash
dotnet restore
```

### 3. Redis Kurulumu:

Redis'i Docker kullanarak çalıştırmak için:

```bash
docker run --name redis -p 6379:6379 -d redis
```

Alternatif olarak, bulut tabanlı bir Redis örneği kullanabilir ve `appsettings.json` dosyasındaki bağlantı dizesini güncelleyebilirsiniz.

### 4. Uygulamayı Çalıştırma:

Uygulamayı çalıştırmak için aşağıdaki komutu kullanabilirsiniz:

```bash
dotnet run
```

Bu, API sunucusunu `http://localhost:5259` (veya yapılandırılan diğer port) üzerinde başlatacaktır.

## Projede Yapılan İşlemler

### RedisCacheService
RedisCacheService, Redis ile etkileşimde bulunarak verileri depolar, alır ve siler. Bu servis, hem basit hem de karmaşık veri türlerini önbelleğe almayı destekler.

### RedisStreamWorker
RedisStreamWorker, Redis Streams kullanarak verileri okuyan ve işlemi gerçekleştiren bir arka plan işleyicisidir.
Bu servis, verileri alırken oluşabilecek hatalar için yeniden deneme mekanizması ve Dead Letter Queue (DLQ) kullanır.

## Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Daha fazla bilgi için [LICENSE](LICENSE) dosyasını inceleyebilirsiniz.

