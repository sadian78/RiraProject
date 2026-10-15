# مستندات پروژه مدیریت کاربران با معماری تمیز و gRPC (RiraProject)

این پروژه پیاده‌سازی سرویس مدیریت کاربران (User Management Service) مبتنی بر استانداردهای **Clean Architecture** و اصول **Domain-Driven Design (DDD)** به همراه ارتباطات **gRPC** در بستر **NET 9.** است.

## ۱. ساختار معماری (Architecture Overview)

سلوشن به صورت لایه‌بندی‌شده و ماژولار تفکیک شده است:

```
RiraProject/
│
├── Src/
│   ├── Framework/                                 # کتابخانه‌های پایه و مشترک (Shared Kernel)
│   │   ├── RiraProject.Framework.Domain           # اینترفیس‌ها، رکوردهای پایه Result و Entity
│   │   ├── RiraProject.Framework.Application      # اینترفیس‌های مشترک لایه اپلیکیشن
│   │   ├── RiraProject.Framework.Infrastructure   # پیاده‌سازی‌های پایه دیتابیس
│   │   └── RiraProject.Framework.Presentation     # تنظیمات مشترک رابط کاربری
│   │
│   └── Services/Users/                            # مایکروسرویس/ماژول مدیریت کاربران
│       ├── RiraProject.Users.Domain               # انتیتی User، مقادیر شیء (Value Objects) و رویدادها
│       ├── RiraProject.Users.Application          # سرویس‌ها، قراردادها و DTOهای بیزینس
│       ├── RiraProject.Users.Infrastructure       # نگاشت‌های EF Core، UserDbContext و ریپازیتوری
│       └── RiraProject.Users.Presentation         # قراردادهای Proto، سرویس‌های gRPC و نقطه شروع (Startup)

```

### ویژگی‌های کلیدی دامنه (DDD & Domain):

* **Value Objects**: مقادیر `NationalCode` (دارای الگوریتم چکسام ریاضی)، `FirstName`، `LastName`، `BirthDate` و `UserId`.

* **Result Pattern**: عدم استفاده از Throw Exception در لایه‌های دامنه و اپلیکیشن برای کنترل خطاهای مورد انتظار.

* **EF Core Fluent API**: استفاده از `HasConversion` برای نگاشت اتوماتیک Value Objectها به انواع ابتدایی در دیتابیس.

* **gRPC Error Handling**: مپ کردن کدهای خطای دامنه به کدهای استاندارد gRPC (`InvalidArgument`, `NotFound`, `AlreadyExists`).

## ۲. پیش‌نیازها

* نصب **NET 9.0 SDK.**

* نرم‌افزار **SQL Server** یا نسخه **SQLEXPRESS / LocalDB**

* ابزار **Postman** (نسخه‌های جدید با پشتیبانی از gRPC) یا ابزار **BloomRPC / grpcui**

## ۳. تنظیم رشته اتصال (Connection String)

در فایل `appsettings.json` پروژه `RiraProject.Users.Presentation` رشته اتصال به دیتابیس را بررسی کنید:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=RiraUserDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True"
  }
}

```

## ۴. ایجاد و مهاجرت دیتابیس (Migration)

برای اعمال جداول روی دیتابیس، یکی از دو روش زیر را انتخاب کنید:

### روش اول: Package Manager Console در Visual Studio

1. پروژه استارتاپ را روی **`RiraProject.Users.Presentation`** قرار دهید.

2. در کنسول، مقدار **Default Project** را روی **`RiraProject.Users.Infrastructure`** بگذارید.

3. دستورات زیر را اجرا کنید:

```
Add-Migration InitialCreate -Context UserDbContext
Update-Database -Context UserDbContext

```

### روش دوم: استفاده از ترمینال (.NET CLI)

در ریشه پروژه دستورات زیر را اجرا کنید:

```
dotnet ef migrations add InitialCreate --project Src/Services/Users/RiraProject.Users.Infrastructure --startup-project Src/Services/Users/RiraProject.Users.Presentation --context UserDbContext
dotnet ef database update --project Src/Services/Users/RiraProject.Users.Infrastructure --startup-project Src/Services/Users/RiraProject.Users.Presentation --context UserDbContext

```

## ۵. نحوه اجرا و تست با Postman (gRPC)

پروژه را اجرا کنید (`F5` یا `dotnet run`). آدرس سرور (مثلاً `https://localhost:7123`) در دسترس قرار می‌گیرد.

### مراحل فراخوانی در Postman:

1. روی **New > gRPC Request** کلیک کنید.

2. آدرس سرور را وارد نمایید.

3. با توجه به فعال بودن قابلیت **Reflection** در حالت Development، سرویس `user.UserGrpcService` به شکل خودکار بارگذاری می‌شود.

## ۶. نمونه داده‌های تست متدها

### ۱) ایجاد کاربر (`CreateUser`):

```
{
  "first_name": "سارا",
  "last_name": "ییی",
  "national_code": "0012345678",
  "birth_date": "1995-05-15T00:00:00Z"
}

```

### ۲) دریافت با شناسه (`GetUserById`):

```
{
  "id": "e89c6d32-26db-48fb-bc1c-a038bf3bc68e"
}

```

### ۳) دریافت همه کاربران (`GetAllUsers`):

ورودی یک آبجکت خالی (`{}`) است.

### ۴) به‌روزرسانی کاربر (`UpdateUser`):

```
{
  "id": "e89c6d32-26db-48fb-bc1c-a038bf3bc68e",
  "first_name": "سارا",
  "last_name": "احمدی",
  "national_code": "0012345678",
  "birth_date": "1995-05-15T00:00:00Z"
}

```

### ۵) حذف کاربر (`DeleteUser`):

```
{
  "id": "e89c6d32-26db-48fb-bc1c-a038bf3bc68e"
}

```

## ۷. تست مدیریت خطاهای پاسخ (Error Handling Test)

| سناریو | مقدار ارسالی | کد وضعیت مورد انتظار gRPC | 
| ----- | ----- | ----- | 
| **کد ملی نامعتبر** | `national_code: "123"` | `3 INVALID_ARGUMENT` | 
| **کد ملی تکراری** | ارسال مجدد کد ملی ثبت‌شده قبلی | `6 ALREADY_EXISTS` | 
| **تاریخ تولد غیرمجاز** | ارسال تاریخ تولد در آینده | `3 INVALID_ARGUMENT` | 
| **کاربر یافت‌نشد** | ارسال Guid تصادفی به `GetUserById` | `5 NOT_FOUND` | 
