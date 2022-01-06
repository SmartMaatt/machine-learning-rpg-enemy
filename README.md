# System sztucznej inteligencji wrogów w grze cRPG w oparciu o uczenie maszynowe
Celem pracy jest zaprojektowanie oraz implementacja systemu antagonisty gry gatunku cRPG w silniku Unity 3D z wykorzystaniem technologii Sztucznej Inteligencji. Priorytetem jest zapewnienie dostosowania zachowań i umiejętności wroga do otaczającego go środowiska, sytuacji, bądź poziomu wyszkolenia gracza, zarazem mając na uwadze poziom doświadczenia nabytego podczas walki z użytkownikiem bądź innym wrogiem. Wraz z postępem rozgrywki wróg ma rozwijać zdolności przewidywania, podejmowania racjonalnych decyzji oraz wykonywania akcji będących najbardziej słuszną opcją względem danego problemu. System będzie uczył się poprzez kolejne podejścia do interakcji z graczem, rozwijając przy tym swoje umiejętności walki i odpowiedzi na atak ostatecznie stawiając gracza przed coraz większym wyzwaniem.

W projekcie zostały wykorzystane dwa rodzaje Uczenia Maszynowego. Pierwszy z nich stosuje Drzewo Behawioralne służące do podejmowania decyzji oraz wykonywania akcji na podstawie aktualnego stanu środowiska otaczającego postać niegrywalną. Przykładowymi reakcjami są podążanie za graczem, gdy znajdzie się on w zasięgu wzroku modelu, bądź ucieczka w możliwie najbezpieczniejsze miejsce w momencie krytycznie niskiego stanu zdrowia. Model dobiera również swoje reakcje zależnie od sytuacji - swobodne poruszanie się po mapie w przypadku braku przeciwnika w pobliżu. Drugi typ Uczenia Maszynowego oparty jest uczenie ze wzmocnieniem. W tym przypadku przeciwnikowi reprezentującemu model dostarczane są liczne obserwacje dotyczące zarówno jego aktualnego stanu, jaki i otaczającego go środowiska oraz wartości maszyny stanów przeciwnika, bądź gracza. Na podstawie dostarczonych informacji model dokonuje decyzji odnośnie do wyboru jednej z udostępnionych mu akcji, którymi w tym przypadku jest wybór odpowiedniego zaklęcia, bądź jego braku. Wykonywane akcje są następnie oceniane i suma wydzielonych punktów w danym epizodzie służy do optymalizacji decyzyjności w późniejszy potyczkach.

# Projekt Aplikacji w Unity 3D
Projekt został utworzony na wersji Unity 2020.3.19f1. Jego zawartość można znaleźć w folderze "EnemyAI - Unity project".

# Projekt Launchera
Projekt launchera został utworzony w języku C# w szablonie "Aplication windows forms". Jego zawartość można znaleźć w folderze "EnemyAI- Launcher"

# Wymagania dla developera
* Python w wersji 3.6.1 lub nowszej
* Instalacja pluginów Python z pliku EnemyAI - Unity project/requirements.txt
* Unity 3D w wesji 2020.3.19f1 lub nowszej

# Build aplikacji
Procedura wygenerowania buildu aplikacji jest złożona dlatego dostęp do ostatniej, dostępnej wersji znajduje się w poniższym linku.
https://smartmatt.pl/enemy-ai-build

# Wymagania dla Builda
* Python w wersji 3.6.1 lub nowszy
* (Python jest wymagany jedynie w trybie uczenia, aplikacja umożliwia użycie trybu rozgrywki bez posiadanego wymagania.)