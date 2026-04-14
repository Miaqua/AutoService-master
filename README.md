1. Клонировать репозиторий

git clone https://github.com/Miaqua/AutoService.git
cd AutoService

2. Установить зависимости
dotnet restore

3. Применить миграции и создать базу данных
dotnet ef database update

4. Запуск проекта
dotnet run


Если при запуске появляется ошибка подключения к базе, удали файл app.db и снова запусти:
dotnet ef database update
