# Volume rendering
### Projekt realizowany w ramach przedmiotu Studio Projektowe, Informatyka i Systemy Inteligentne V sem. AGH
Renderowanie wolumetryczne to potężna technika w grafice komputerowej, umożliwiająca wizualizację zbiorów danych wolumetrycznych, takich jak obrazy medyczne, symulacje naukowe czy skany przemysłowe. Niniejsza dokumentacja projektu opisuje implementację i użytkowanie systemu renderingu wolumetrycznego, który wykorzystuje najnowsze algorytmy do generowania przekonujących wizualizacji z trójwymiarowych pól skalarów.

## Zrealizowane algorytmy
### Marching Cubes
Algorytm Marching Cubes jest powszechnie używaną i efektywną techniką generowania trójwymiarowej grafiki na podstawie danych przestrzennych. Ten zaawansowany algorytm, zaproponowany pierwotnie przez Lorensa i Cline'a w 1987 roku, jest szczególnie użyteczny w dziedzinie wizualizacji medycznej, geologii, oraz innych dziedzin, gdzie trójwymiarowa reprezentacja danych jest kluczowa. Algorytm Marching Cubes pozwala na wizualizację danych przestrzennych w postaci trójwymiarowych obiektów, a jego popularność wynika z jego zdolności do generowania powierzchni w oparciu o skalarne dane, takie jak gęstość, temperatura czy inne właściwości fizyczne.

### Ray Casting
Ray casting to technika wykorzystująca możliwości szybkiego przetwarzania równoległego przez karty graficzne (GPU). Jej zasadniczym elementem jest wyliczanie koloru każdego piksela tekstury poprzez wysyłanie wirtualnych wiązek w przestrzeni świata. Po wyliczeniu kierunku i trajektorii wiązki, można stwierdzić, przez jakie punkty przechodzi, a w związku z tym stwierdzić, czy napotyka renderowany obiekt. Zaawansowane techniki mające na celi wyliczenie stopnia oświetlenia piksela na podstawie zjawiska załamania / odbicia wiązki noszą nazwę "Ray tracing", a są one wykorzystywane przede wszystkim we współczesnych grach komputerowych i narzędziach typu Blender.

## Użytkowanie
Zawartość folderu algorytmu powinna zostać przeniesiona do folderu "Assets" nowego projektu Unity, zastępując istniejące tam pliki. Projekt działa jak standrdowy projekt w Unity.

## Dokumentacja
Raport z projektu oraz nagrane przykładu użycia znajdują się w folderze Doc.

### Użyte dane pochodzą ze strony [Stanford Computer Graphics Laboratory](https://graphics.stanford.edu/data/voldata/)
