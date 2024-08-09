
Bu solutionda **MediatR** ile **CQRS Pattern** kullanılarak oluşturulan sipariş akışı **EventStore** ile ile microservisler arasında asenkron yönetilerek **Event Sourcing Pattern** implementasyonu yapılmıştır.

**CQRS** (Command Query Responsibility Segregation pattern amacı veri yazma (**Command**) ve veri okuma (**Query**) işlemlerini ayrı sorumluluklarda yönetmektir.

- **Commands**: Sistemde bir değişiklik yapmak veya veriyi güncellemek için kullanılan işlemlerdir. Komutlar, genellikle veri üzerinde bir değişiklik yapar veya bu işlemin sonucunda bir olay oluşturabilir. Bu senaryoda Ürün oluşturma (CreateProductCommand), Ürün Adı güncelleme (UpdateProductNameCommand), Ürün Fiyatı güncelleme (UpdateProductPriceCommand), Sipariş oluşturma (CreateProductCommand) işlemleri Command olarak ele alınırmıştır.
  
- **Queries**: Sistemdeki mevcut verileri okumak ve bilgi almak için kullanılan işlemlerdir. Sorgular, veri üzerinde herhangi bir değişiklik yapmaz, sadece veriyi sorgular ve getirir. Bu senaryoda Ürünleri getirme (GetProductsQuery) ve Siparişleri getirme (GetOrdersQuery) Query olarak ele alınılmıştır


**Event Sourcing Pattern**'de geleneksel veri yönetim yaklaşımlarından farklı olarak, kayıtların güncel durumunu saklamak yerine, verinin zaman içindeki değişiklikleri (**Event**) saklanır. Olaylar sıralı bir şekilde saklandığı için geçmişteki herhangi bir durumuna dönme veya bir olayı yeniden oynatma imkanı sağlanır. Yine olaylar sıralı bir şekilde tutulduğu için olaylar sentezlenip verinin güncel haline ulaşılabilir. **CQRS** ve **Event Sourcing** birlikte kullanıldığında Command işlemlerinde veriyi kaydetmek yerine bu veriye ait olay (**Event**) veritabanına kaydedilir. Olay veritabanına kaydedildikten sonra tetiklenir ve sorguların yapıldığı yerde bu olay dinlenerek verilerin güncel halinin tutulduğu veri tabanında güncelleme yapılır.

![image](https://github.com/user-attachments/assets/cb9e6293-f086-4e5e-8f35-c32adce7a699)

**Event Store**, Event Sourcing Pattern'de kullanılan olayları sıralı bir şekilde saklayıp yönetimini yönetim sağlayan ve aynı zmanada Message Broker olarak hizmet veren bir yapıdır. 

EventStore Docket Desktop üzerinden aşağıdaki komut ile ayağa kaldırılabilir.

	docker run -d --name esdb-node -p 2113:2113 -p 1113:1113 `
    eventstore/eventstore:latest --insecure --run-projections=All 

EventStore bağlantısı için EventStore.Client.Grpc kütüphanesi kullanılmıştır.

![image](https://github.com/user-attachments/assets/0499dd73-a0a1-421b-ac01-3fa1ea5801f3)
    
Bu senaryoda Command servisinde her bir Command için EventStore Client ile AppendToStream metodu ile Event'lar EventStore'a gönderilmiştir. Command içerisinde herhangi bir veri tabanı operasyonu yoktur, gelen request bir event haline getirilip EventStore'a gönderilir.

![image](https://github.com/user-attachments/assets/791d2923-2e7b-4def-bff6-237ce1c88a4c)

Query servisinde EventStore'a Subscribe implementasyonu yazılmıştır. SubscribeToAll ile EventStore'a subscribe olunur. Bu senaryoda sadece yeni gelen eventlara subscribe olunmuştur, ayarlar yapılandırılarak gelmiş geçmiş bütün eventlara veya berlirli bir olay anına buradan projeksiyon tutulabilir.

![image](https://github.com/user-attachments/assets/4eef98fb-c993-4b32-9972-c57b35ed72a0)

Subscribe olduktan sonra gelen Event işlenerek Query veritabanında ilgili güncelleme yapılır.

![image](https://github.com/user-attachments/assets/3bdea759-55fa-42ea-83f1-b2ea8eec4fed)



