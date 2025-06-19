EXTERNAL SetPortrait(name)
EXTERNAL PlayMusic(name)
#using SetPortrait
#using PlayMusic

Widzisz Alice wymachującą w powietrzu mieczem 
…
Po chwili w końcu ciebie zauważa 
~SetPortrait("Alice")
…
~SetPortrait("")
Zdaje się że jest na ciebie zezłoszczona 
~PlayMusic("Trouble")
Szybko! Powiedz coś zanim cię zdekapituje!
*[Ummmm Czeeeeśććć?]
~SetPortrait("Alice")
->Hello
*[Przepraszam że się spóźniłem ale…]
~SetPortrait("Alice")
->But
*[Wiem gdzie odbędzie się inicicjacja!]
~SetPortrait("Alice")
->Survive

===Hello===
Cześć?!!!
->Yelling

===But===
Żadnego ale!
->Yelling

===Survive===
Nie chce słyszeć żadnych twoich...
Czekaj...
Co?!!!
Wiesz gdzie odbędzie się inicjacja?!!!
~PlayMusic("AliceMeadow")
Spóźniłeś się z jakieś 5 godzin ale powiedzmy że ci wybaczę
Nie zmarnowałeś chociaż tego czasu
To gadaj, wiesz na co będziemy polować?
~SetPortrait("Fex")
Nie znam szczegółow, jedyne czego udało mi się dowiedzieć to że miejscem polowania ma być ponoć opuszczona kopalnia 
W ramach przygotowań czytałem trochę na jej temat i głównie zamieszkiwana jest aktualnie przez mniejsze potwory typu ziemistych 
~SetPortrait("Alice")
->Proposition


===Yelling===
Czy ty zdajesz sobie sprawę jak późno jest?!
Naprawdę chociaż jeden raz w swoim życiu mógłbyś pojawić się na czas
Mieliśmy ćwiczyć od wschodu słońca!
Od wschodu!
A jest południe
Dobra
Skończyłam 
->Proposition

===Proposition===
~PlayMusic("AliceMeadow")
Ok, nie mamy już czasu na poważniejszy trening więc co ty na to żeby znaleźć jakiegoś małego potwora i przypomnieć sobie na szybko wszystko w praktyce? 
*[Jasne]
->LetsGo
*[Jesteś pewna że to dobry pomysł?]
->ComeOn


===LetsGo===
To dajesz idziemy!
->End

===ComeOn===
No weź
Jak nie poradzimy sobie z jakimś potworkiem w lesie to tym bardziej nie damy sobie radę na dzisiejszym polowaniu
W zeszłym roku kandydaci musieli ponoć upolować elektrycznego boa
No chodź! 
->End

===End===
~SetPortrait("")
Alice dołączyła do twojej drużyny! 
->END

