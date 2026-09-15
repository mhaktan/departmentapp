# departmentapp — Talep Analizi

> Bu belge Archipid talep olgunlaştırma (discovery) akışıyla üretildi.

## Orijinal Talep

Kurumsal bir "Satın Alma Talep ve Onay Yönetim Sistemi" istiyorum.

## Master data / parametre entity'leri:

- Department (Birim): birim kodu, birim adı, yıllık bütçe, aktif/pasif
- Employee (Personel): sicil numarası, ad soyad, e-posta, birim (Department
  ilişkili), unvan, aktif/pasif
- Supplier (Tedarikçi): tedarikçi kodu, tedarikçi adı, vergi numarası,
  iletişim kişisi, e-posta, aktif/pasif
- ExpenseCategory (Harcama Kalemi): kalem kodu, kalem adı (örn. BT Donanım,
  Danışmanlık, Sarf Malzeme), aktif/pasif

## Operasyonel entity'ler:

- PurchaseRequest (Satın Alma Talebi): talep numarası, talep eden (Employee
  ilişkili), talep eden birim (Department ilişkili), harcama kalemi
  (ExpenseCategory ilişkili), gerekçe, toplam tutar, ihtiyaç tarihi,
  red gerekçesi, durum
- PurchaseRequestItem (Talep Kalemi): talep (PurchaseRequest ilişkili),
  ürün/hizmet adı, miktar, birim fiyat, satır tutarı — bir talepte birden
  fazla kalem olabilir
- Quotation (Teklif): talep (PurchaseRequest ilişkili), tedarikçi (Supplier
  ilişkili), teklif tutarı, teklif tarihi, seçildi mi
- PurchaseOrder (Sipariş): talep (PurchaseRequest ilişkili), sipariş numarası,
  tedarikçi (Supplier ilişkili), sipariş tarihi, teslim tarihi, sipariş tutarı

Toplam 8 entity olsun, fazlasını ekleme.

## RBAC rolleri:

Requester (Talep Eden), DepartmentManager (Birim Müdürü), FinanceApprover
(Finans Onaycısı), PurchasingOfficer (Satın Alma Sorumlusu) sistem rolleri olsun.

## Onay akışı (state machine) — PurchaseRequest için:

Draft → PendingManagerApproval → PendingFinanceApproval → Approved → Ordered

Onay adımları ROL bazlı olsun:

- PendingManagerApproval adımı DepartmentManager rolüne atansın.
- PendingFinanceApproval adımı FinanceApprover rolüne atansın.
- Approved → Ordered geçişini PurchasingOfficer rolü yapsın.
- Herhangi bir onaycı reddederse talep Draft'a geri dönsün (revizyon) ve
  red gerekçesi zorunlu olsun.

Kural: Draft'tan onaya göndermek için talebin en az bir PurchaseRequestItem
kaydı bulunsun.
Kural: Approved'dan Ordered'a geçmek için talebin seçilmiş en az bir Quotation
kaydı bulunsun.

## Dashboard:

- Onayımı bekleyen talep sayısı
- Durum bazında talep dağılımı
- Birim bazında talep tutarı dağılımı
- Ortalama onay süresi (gün) — talep tarihinden onay tarihine
- İhtiyaç tarihi geçmiş, hâlâ onaylanmamış talepler listesi

## Özet

Bu uygulama, kurum çalışanlarının satın alma taleplerini dijital ortamda oluşturmasını, birim müdürü ve finans ekibinin sırayla onaylamasını, satın alma sorumlusunun tedarikçilerden teklif alıp sipariş oluşturmasını ve oluşturulan siparişlerin satın alma müdürü tarafından onaylanmasını sağlar. Tüm süreç kayıt altına alınır ve şeffaf bir şekilde izlenebilir.

Çalışan bir satın alma talebi oluşturur, ürün/hizmet kalemlerini ekler ve onaya gönderir. İlk olarak birim müdürü talebi inceler; onaylarsa finans ekibine geçer, reddederse talep gerekçeyle birlikte çalışana geri döner. Finans onayı tamamlandığında talep 'Onaylandı' durumuna geçer. Satın alma sorumlusu tedarikçilerden teklif alır, uygun teklifi seçer ve siparişi oluşturur. Sipariş, satın alma müdürüne onay için gönderilir; onaylanırsa süreç tamamlanır, revize istenirse sipariş düzenlenerek yeniden onaya sunulur.

## Kapsam

- (belirtilmedi)

## Kapsam Dışı

- (belirtilmedi)

## Açık Noktalar

- (belirtilmedi)

## Eksiklikler

- (belirtilmedi)

## Öneriler

- (belirtilmedi)

## Veri Modeli

```mermaid
erDiagram
    User ||--o{ Employee : "1:N"
    Department ||--o{ Employee : "1:N"
    Employee ||--o{ PurchaseRequest : "1:N"
    Department ||--o{ PurchaseRequest : "1:N"
    ExpenseCategory ||--o{ PurchaseRequest : "1:N"
    PurchaseRequest ||--o{ PurchaseRequestItem : "1:N"
    PurchaseRequest ||--o{ Quotation : "1:N"
    Supplier ||--o{ Quotation : "1:N"
    PurchaseRequest ||--o{ PurchaseOrder : "1:N"
    Supplier ||--o{ PurchaseOrder : "1:N"
    User {
        long id PK
        string userName "zorunlu"
        string emailAddress "zorunlu"
        string name "opsiyonel"
        string surname "opsiyonel"
        bool isActive "opsiyonel"
    }
    Department {
        long id PK
        string code "zorunlu"
        string name "zorunlu"
        decimal annualBudget "opsiyonel"
        bool isActive "zorunlu"
    }
    Employee {
        long id PK
        string registrationNumber "zorunlu"
        string fullName "zorunlu"
        string email "zorunlu"
        string title "opsiyonel"
        bool isActive "zorunlu"
    }
    Supplier {
        long id PK
        string code "zorunlu"
        string name "zorunlu"
        string taxNumber "opsiyonel"
        string contactPerson "opsiyonel"
        string email "opsiyonel"
        bool isActive "zorunlu"
    }
    ExpenseCategory {
        long id PK
        string code "zorunlu"
        string name "zorunlu"
        bool isActive "zorunlu"
    }
    PurchaseRequest {
        long id PK
        string requestNumber "zorunlu"
        string justification "zorunlu"
        decimal totalAmount "opsiyonel"
        DateTime neededDate "zorunlu"
        DateTime approvedDate "opsiyonel"
        string rejectionReason "opsiyonel"
        enum status "zorunlu"
    }
    PurchaseRequestItem {
        long id PK
        string productName "zorunlu"
        decimal quantity "zorunlu"
        decimal unitPrice "zorunlu"
        decimal lineTotal "opsiyonel"
    }
    Quotation {
        long id PK
        DateTime quotationDate "zorunlu"
        decimal amount "zorunlu"
        bool isSelected "zorunlu"
    }
    PurchaseOrder {
        long id PK
        string orderNumber "zorunlu"
        DateTime orderDate "zorunlu"
        DateTime deliveryDate "opsiyonel"
        decimal orderAmount "zorunlu"
        string notes "opsiyonel"
        string rejectionReason "opsiyonel"
        enum status "zorunlu"
    }
```

### User — Kullanıcı (Sistem)

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `userName` | string | Evet | 64 |
| `emailAddress` | string | Evet | 256 |
| `name` | string | Hayır | 128 |
| `surname` | string | Hayır | 128 |
| `isActive` | bool | Hayır | — |

**Neye bağlı:** Employee (1:N, bu tablo "bir" tarafı)

### Department — Birim

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `code` | string | Evet | 20 |
| `name` | string | Evet | 200 |
| `annualBudget` | decimal | Hayır | — |
| `isActive` | bool | Evet | — |

**Neye bağlı:** Employee (1:N, bu tablo "bir" tarafı) · PurchaseRequest (1:N, bu tablo "bir" tarafı)

### Employee — Personel

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `registrationNumber` | string | Evet | 50 |
| `fullName` | string | Evet | 200 |
| `email` | string | Evet | 256 |
| `title` | string | Hayır | 100 |
| `isActive` | bool | Evet | — |

**Neye bağlı:** User (1:N, bu tablo "çok" tarafı) · Department (1:N, bu tablo "çok" tarafı) · PurchaseRequest (1:N, bu tablo "bir" tarafı)

### Supplier — Tedarikçi

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `code` | string | Evet | 20 |
| `name` | string | Evet | 200 |
| `taxNumber` | string | Hayır | 20 |
| `contactPerson` | string | Hayır | 200 |
| `email` | string | Hayır | 256 |
| `isActive` | bool | Evet | — |

**Neye bağlı:** Quotation (1:N, bu tablo "bir" tarafı) · PurchaseOrder (1:N, bu tablo "bir" tarafı)

### ExpenseCategory — Harcama Kalemi

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `code` | string | Evet | 20 |
| `name` | string | Evet | 200 |
| `isActive` | bool | Evet | — |

**Neye bağlı:** PurchaseRequest (1:N, bu tablo "bir" tarafı)

### PurchaseRequest — Satın Alma Talebi

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `requestNumber` | string | Evet | 50 |
| `justification` | string | Evet | 1000 |
| `totalAmount` | decimal | Hayır | — |
| `neededDate` | DateTime | Evet | — |
| `approvedDate` | DateTime | Hayır | — |
| `rejectionReason` | string | Hayır | 1000 |
| `status` | enum (Draft,PendingManagerApproval,PendingFinanceApproval,PendingDirectorApproval,Approved,Ordered) | Evet | — |

**Neye bağlı:** Employee (1:N, bu tablo "çok" tarafı) · Department (1:N, bu tablo "çok" tarafı) · ExpenseCategory (1:N, bu tablo "çok" tarafı) · PurchaseRequestItem (1:N, bu tablo "bir" tarafı) · Quotation (1:N, bu tablo "bir" tarafı) · PurchaseOrder (1:N, bu tablo "bir" tarafı)

### PurchaseRequestItem — Talep Kalemi

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `productName` | string | Evet | 300 |
| `quantity` | decimal | Evet | — |
| `unitPrice` | decimal | Evet | — |
| `lineTotal` | decimal | Hayır | — |

**Neye bağlı:** PurchaseRequest (1:N, bu tablo "çok" tarafı)

### Quotation — Teklif

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `quotationDate` | DateTime | Evet | — |
| `amount` | decimal | Evet | — |
| `isSelected` | bool | Evet | — |

**Neye bağlı:** PurchaseRequest (1:N, bu tablo "çok" tarafı) · Supplier (1:N, bu tablo "çok" tarafı)

### PurchaseOrder — Sipariş

| Alan | Tip | Zorunlu | Uzunluk |
|---|---|---|---|
| `id` | long | Evet | — |
| `orderNumber` | string | Evet | 50 |
| `orderDate` | DateTime | Evet | — |
| `deliveryDate` | DateTime | Hayır | — |
| `orderAmount` | decimal | Evet | — |
| `notes` | string | Hayır | 1000 |
| `rejectionReason` | string | Hayır | 1000 |
| `status` | enum (Draft,PendingApproval,Approved,Rejected) | Evet | — |

**Neye bağlı:** PurchaseRequest (1:N, bu tablo "çok" tarafı) · Supplier (1:N, bu tablo "çok" tarafı)


## İş Akışları

### Satın Alma Talebi — durum makinesi

```mermaid
stateDiagram-v2
    [*] --> Draft
    Draft --> PendingManagerApproval : Submit
    PendingManagerApproval --> PendingFinanceApproval : Approve
    PendingManagerApproval --> Draft : Revise
    PendingFinanceApproval --> PendingDirectorApproval : Approve
    PendingFinanceApproval --> Draft : Revise
    PendingDirectorApproval --> Approved : Approve
    PendingDirectorApproval --> Draft : Revise
    Approved --> Ordered : PlaceOrder
    Ordered --> [*]
```

**Onay adımları**

| # | Adım | Atanan | Aksiyonlar | Zorunlu alanlar |
|---|---|---|---|---|
| 1 | Birim Müdürü Onayı | Rol: `DepartmentManager` | Onayla (approve), Reddet (revise) | `rejectionReason` |
| 2 | Finans Onayı | Rol: `FinanceApprover` | Onayla (approve), Reddet (revise) | `rejectionReason` |
| 3 | Direktör Onayı | Rol: `Director` | Onayla (approve), Reddet (revise) | `rejectionReason` |

Reddedilirse kayıt **`Draft`** durumuna döner.

### Sipariş — durum makinesi

```mermaid
stateDiagram-v2
    [*] --> Draft
    Draft --> PendingApproval : Submit
    PendingApproval --> Approved : Approve (PurchasingManager)
    PendingApproval --> Draft : Revise (PurchasingManager)
    Draft --> Rejected : Reject
    Approved --> [*]
    Rejected --> [*]
```

**Onay adımları**

| # | Adım | Atanan | Aksiyonlar | Zorunlu alanlar |
|---|---|---|---|---|
| 1 | Satın Alma Onayı | Rol: `PurchasingManager` | Onayla (approve), Revize Et (revise) | `rejectionReason` |

Reddedilirse kayıt **`Draft`** durumuna döner.

### Akış: PurchaseRequest Approval Flow

Auto-generated approval flow for PurchaseRequest. Customize email templates and add conditions as needed.

```mermaid
flowchart TD
    PurchaseRequest_approval_trigger(["On PurchaseRequest Submit"])
    PurchaseRequest_approval_condition{"Status = PendingManagerApproval?"}
    PurchaseRequest_approval_approval[["PurchaseRequest Approval"]]
    PurchaseRequest_approval_email["Send Approval Email (send-email)"]
    PurchaseRequest_approval_completion_trigger(["On PurchaseRequest Approved"])
    PurchaseRequest_approval_completion_email["Send Completion Email (send-email)"]
    PurchaseRequest_approval_trigger --> PurchaseRequest_approval_condition
    PurchaseRequest_approval_condition -->|true| PurchaseRequest_approval_approval
    PurchaseRequest_approval_approval --> PurchaseRequest_approval_email
    PurchaseRequest_approval_completion_trigger --> PurchaseRequest_approval_completion_email
```

### Akış: PurchaseOrder Approval Flow

Auto-generated approval flow for PurchaseOrder. Customize email templates and add conditions as needed.

```mermaid
flowchart TD
    PurchaseOrder_approval_trigger(["On PurchaseOrder Submit"])
    PurchaseOrder_approval_condition{"Status = PendingApproval?"}
    PurchaseOrder_approval_approval[["PurchaseOrder Approval"]]
    PurchaseOrder_approval_email["Send Approval Email (send-email)"]
    PurchaseOrder_approval_completion_trigger(["On PurchaseOrder Approved"])
    PurchaseOrder_approval_completion_email["Send Completion Email (send-email)"]
    PurchaseOrder_approval_trigger --> PurchaseOrder_approval_condition
    PurchaseOrder_approval_condition -->|true| PurchaseOrder_approval_approval
    PurchaseOrder_approval_approval --> PurchaseOrder_approval_email
    PurchaseOrder_approval_completion_trigger --> PurchaseOrder_approval_completion_email
```


## Elle Geliştirme Gerektirenler

Aşağıdaki maddeler senaryonun gereği ama üretilen koda yansımıyor — kod yazılması gerekir.

| Alan | İş | Neden | Geçici çözüm |
|---|---|---|---|
| entity | Toplam tutar ve satır tutarı otomatik hesaplanmaz | lineTotal (miktar × birim fiyat) ve PurchaseRequest.totalAmount (kalemlerin toplamı) hesaplanmış alanlar; platform düz saklar, otomatik hesaplama üretilmez. | lineTotal ve totalAmount normal alanlar olarak bırakıldı; kullanıcı girer veya ileride özel geliştirmeyle hesaplanabilir. |
| approval | Seçilmiş teklif sayısı guard'ı yalnızca 'Quotation kaydı var mı' kontrolü yapar | child-exists guard'ı bağlı kayıt varlığını kontrol eder; isSelected=true filtresini değerlendiremez. Approved→Ordered geçişi için guard 'en az 1 Quotation kaydı' olarak kuruldu, isSelected=true koşulu denetlenemez. | PurchasingOfficer sipariş oluşturmadan önce teklif seçimini manuel doğrular. |
| entity | Talep ve sipariş numarası otomatik sıralı üretilmez | Sequence/numaratör üretimi desteklenmiyor; requestNumber ve orderNumber düz string alan olarak saklanır. | Kullanıcı kendi numarasını girer. |
| flow | Sipariş oluşunca talebin durumu otomatik Ordered'a geçmez | Çapraz kayıt otomasyonu (PurchaseOrder kaydedilince PurchaseRequest.status güncellenmesi) desteklenmiyor. | PurchasingOfficer sipariş oluşturduktan sonra talep üzerinde 'PlaceOrder' geçişini manuel tetikler. |
