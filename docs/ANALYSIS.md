# departmentapp — Talep Analizi

> Bu belge Archipid talep olgunlaştırma (discovery) akışıyla üretildi.

## Orijinal Talep

bir insan kaynakları yönetim portalı istiyorum

## Özet

Bu uygulama, şirketin insan kaynakları süreçlerini uçtan uca dijital olarak yönetir. Personel özlük bilgilerinden izin yönetimine, işe alımdan performans değerlendirmeye, eğitim takibinden maaş kayıtlarına, fazla mesai girişinden disiplin kayıtlarına kadar tüm İK operasyonlarını tek bir platformda bir araya getirir.

İK Uzmanı yeni personel kaydı oluşturur; kişisel bilgi, iletişim, sertifikalar ve maaş bilgileri sisteme işlenir. Personel izin talebi oluşturduğunda önce departman müdürü onaylar; izin türü İK onayı gerektiriyorsa ardından İK uzmanı da onaylar — herhangi biri revize isterse form oluşturana geri döner. Yılda bir (veya dönemsel) performans değerlendirmesi açılır: çalışan öz değerlendirme yapar, yönetici puan ekler, seçilen akranlar anonim geri bildirim verir, İK nihai onayı verir. İşe alımda ilan yayımlanır, başvurular ön eleme → mülakat → teklif aşamalarından geçer; teklif kabul edilince oryantasyon başlar ve yeni personel kaydına dönüşür. Eğitimler plan bazında takip edilir; katılım ve sertifika bilgileri personel dosyasına işlenir. Departman müdürleri çalışan için fazla mesai kaydını onaylar; disiplin olayları da kayıt altına alınır.

## Kapsam

- Personel sicil yönetimi — kişisel bilgi, iletişim, sertifika, acil durum kişisi, banka/vergi bilgisi
- İzin yönetimi — requiresHRApproval bayrağına göre tek veya çift kademeli onay; onay sonrası bakiye otomatik düşülür
- Performans değerlendirme — öz değerlendirme → yönetici → peer (yönetici atar) → İK zinciri
- İşe alım — ilan, başvuru, mülakat, teklif, onboarding ve personel kaydına dönüşüm
- Maaş kaydı — brüt/net maaş ve kesinti takibi (tarihsel geçmiş korunur)
- Eğitim yönetimi — plan, eğitim takibi, katılım ve sertifika
- Fazla mesai takibi — çalışan girişi + Departman Müdürü onay akışı
- Disiplin / uyarı kayıtları — itiraz akışı: Open→UnderReview→Resolved veya Appealed→Closed; Appealed aşamasını İK Müdürü çözümler
- Çok şubeli / çok departmanlı yapı — Departman Müdürü → İK Uzmanı hiyerarşisi

## Kapsam Dışı

- Dosya / belge eki yükleme (CV, sertifika belgesi, ödeme makbuzu vb.) — bu sürümde desteklenmiyor
- SMS veya anlık (push) bildirim — yalnızca e-posta bildirimi desteklenmektedir
- Bordro hesaplama motoru (vergi, SGK, net maaş otomatik hesaplama) — yalnızca maaş kaydı tutulmaktadır
- Fazla mesainin maaş kaydına otomatik yansıtılması — kayıt tutulur ancak hesaplama motoru yoktur
- Gerçek zamanlı raporlama / dashboard / analitik — kapsam dışında
- Takvim entegrasyonu ve izin görselleştirmesi — kapsam dışında

## Açık Noktalar

- (belirtilmedi)

## Eksiklikler

- requiresHRApproval bayrağı LeaveType üzerinde tutuluyor; sistem kurulumunda İK ekibi her izin türü için bu değeri doğru set etmeli
- annualLeaveBalance otomatik düşülmesi için onay sonrası bir tetikleyici gerekiyor — bu backend tarafında event-driven olarak çalışacak, UI'da anlık yansımayacak
- DisciplinaryRecord'da 'acknowledgedByEmployee' alanı var ancak çalışanın elektronik onay aksiyonu modellenmedi; bu alan manuel olarak İK tarafından işaretlenir
- PeerReview reviewer'ları yönetici tarafından atanıyor; bu atama şu an PerformanceReview üzerinden ilişki ile yönetilmektedir — ayrı bir 'PeerReviewAssignment' entity'si gelecekte düşünülebilir

## Öneriler

- LeaveType seed datasını İK Müdürü ile birlikte hazırlayın; requiresHRApproval değerini her tür için netleştirin (yıllık: true, mazeret: false gibi)
- DisciplinaryRecord Appealed → Closed geçişini İK Müdürü rolüne atayın; bu sayede sıradan İK uzmanları itirazı kapatamazlar
- PeerReview atamalarının yönetici tarafından yapıldığını süreç dokümanlarına not düşün; sistem otomatik atama yapmıyor
- Maaş ve disiplin kayıtları hassas veri içerdiğinden rol/izin konfigürasyonunu kurulum öncesi gözden geçirin
