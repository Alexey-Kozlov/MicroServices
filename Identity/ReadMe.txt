SPA c реактом в Visual Studio создается некорректно. С Angular - да, это работает, корректно
создаются компоненты. Но для React - шабонов компонентов нет и IntelliSence работает коряво.
Поэтому делаем так - если необходимо сделать SPA в VS с Реактом - создаем отдельно 2 проекта.
Один - WebApi
Второй - standalone React with TypeScript
После этого отлаживаем реакт-приложение.

Потом внедрянем отлаженное приложение на React в WebApi
Переходим в каталог WebApi и там вводим комагду ;
npx create-react-app client-app --use-npm --template typescript

В каталоге WebApi в результате создастся пустое React-приложение на Typescript
 
Если запустить проект - будет предупреждение чтото вроде - "...onaftersetupmiddleware option is deprecated ..."
Замещаем в папке D:\Projects\IdentityReact\IdentityReact\ClientApp\node_modules\react-scripts\config файл webpackDevServer.config.js, его замещаем из сохраненного

Ставим нужные пакеты, удаляем ненужные (например, web-vitals)

Далее копируем все компоненты в новое реакт-приложение.
Проверяем - npm start, правим ошибки
Создаем в каталоге WebApi папку wwwroot - там будет располагаться сбилденное реакт-приложение
В каталоге client-app создаем файл конфигурации реакта, вот примерная конфигурация:

REACT_APP_IDENTITY=https://192.168.1.10:5010
REACT_APP_FRONT=https://192.168.1.10:3000
PORT=5010
HOST=192.168.1.10
HTTPS=true
SSL_CRT_FILE=cert/192_168_1_10.crt
SSL_KEY_FILE=cert/192_168_1_10.key
BUILD_PATH=../wwwroot

Это переменные, конфигурация хоста и порта, папка с сертификатами для HTTPS и важное -
переменная BUILD_PATH - указывает куда будут копируоваться релизные файлы после билда.
В нашем случае - это созданная папка wwwroot.

Конфигурируем WebApi для работы с реакт-приложением.
Для этого в Program (перед app.Run()) добавляем:
app.MapFallbackToController("Index", "Fallback");

Добавляем новый контроллер FallbackController, в нем единственный метод:

return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");

Этот клонтроллер указыванет на папку wwwroot с реакт-приложением
Важно - класс этого контроллера пометить -     [AllowAnonymous] для доступа.
Вобщем все.
Делаем npm run build - в папке wwwroot должен оказаться релиз реакта
Запускаем WebApi - dotnet run, проверяем - реакт должен откликаться по настроенным в нем роутам.
