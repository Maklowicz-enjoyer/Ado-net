========================================================================
DOKUMENTACJA UŻYTKOWNIKA I TECHNICZNA: WYPOŻYCZALNIA FILMÓW (v1.0)
========================================================================

1. OPIS PROJEKTU
----------------
Aplikacja "Wypożyczalnia filmów" to system typu desktop (Windows Forms) 
stworzony w technologii .NET z wykorzystaniem ADO.NET, służący do 
zarządzania procesem wypożyczania i zwrotów filmów. System integruje się 
z bazą danych PostgreSQL.

2. KOMPONENTY PODSTAWOWE (NIEZBĘDNE)
------------------------------------
Aplikacja do poprawnego działania wymaga następujących komponentów:

A. Środowisko uruchomieniowe:
   - .NET 8.0 (Windows Desktop Runtime).

B. Baza danych:
   - Serwer PostgreSQL (domyślnie skonfigurowany na host: 100.80.77.35).

C. Biblioteki zewnętrzne (NuGet):
   - Npgsql (obsługa połączenia i komend SQL).

D. Komponenty UI (WinForms):
   - DataGridView (prezentacja danych w tabelach).
   - BindingSource i BindingNavigator (nawigacja i wiązanie danych).
   - TabControl (podział na Katalog i Moje Wypożyczenia).

3. WYTYCZNE DOTYCZĄCE KONFIGURACJI
----------------------------------
Przed pierwszym uruchomieniem należy przeprowadzić konfigurację połączenia:

1. Plik hasła: W folderze z plikiem wykonywalnym (.exe) MUSI znajdować się 
   plik tekstowy o nazwie 'haslo.txt'. Powinien on zawierać wyłącznie 
   hasło do bazy danych (plain text).
2. Parametry bazy: W klasie 'Database.cs' zdefiniowane są parametry takie jak 
   Host, Port, DbName oraz User. W razie zmiany serwera, należy je 
   zaktualizować w kodzie źródłowym.

4. INSTRUKCJA OBSŁUGI APLIKACJI
-------------------------------

A. Katalog filmów (Zakładka 1):
   - Wyszukiwanie: W polu na górze wpisz tytuł lub gatunek filmu. Lista 
     filtruje się automatycznie podczas pisania.
   - Wypożyczanie: Zaznacz wybrany film w tabeli i kliknij przycisk 
     "Wypożycz zaznaczony". 
   - Limity: Jeden klient może mieć maksymalnie 3 aktywne wypożyczenia.
   - System sprawdza dostępność kopii przed zatwierdzeniem operacji.

B. Moje wypożyczenia (Zakładka 2):
   - Przeglądanie: Tabela wyświetla aktualnie wypożyczone filmy, terminy 
     zwrotu oraz ewentualne naliczone kary.
   - Zwrot filmu: Zaznacz film i kliknij "Zwróć zaznaczony". 
   - Kary: Jeśli termin zwrotu minął, system automatycznie wyliczy karę 
     na podstawie ceny za dzień i liczby dni spóźnienia.

5. ARCHITEKTURA DANYCH I BEZPIECZEŃSTWO
---------------------------------------
- Transakcje: Kluczowe operacje (wypożyczenie/zwrot) są realizowane w ramach 
  transakcji ADO.NET (NpgsqlTransaction). W przypadku błędu, zmiany są 
  cofane (Rollback).
- Mapowanie danych: Dane z bazy są pobierane do obiektów DataTable 
  i DataSet za pośrednictwem DataAdaptera.
- Obsługa błędów: Aplikacja posiada wbudowaną obsługę wyjątków, wyświetlając 
  użytkownikowi czytelne komunikaty w przypadku problemów z bazą.