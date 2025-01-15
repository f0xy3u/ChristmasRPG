# RPG Survival Game

Textová RPG hra napsaná v C# s tahovými souboji, správou inventáře a systémem ukládání.

## Funkce

- **Systém soubojů**
    - Tahové bitvy proti různým bossům
    - Systém kritických zásahů
    - Více nepřátel v jednom souboji
    - Možnost útěku z bitev

- **Správa inventáře**
    - Zbraně s různými hodnotami útoku
    - Léčivé lektvary
    - Nákup/prodej předmětů v obchodě

- **Systém ukládání**
    - Ukládání/nahrávání postupu ve hře
    - Podpora více souborů s uloženými hrami
    - Trvalé statistiky hráče a inventář

## Herní obsah

### Zbraně
- Jmelová dýka (5 DMG)
- Meč z cukrovinky (10 DMG)
- Slavnostní luk (20 DMG)
- Louskáčkový palcát (25 DMG)
- Sekera mrazivého obra (35 DMG)
- Hůlka svaté hvězdy (50 DMG)

### Lektvary
- Léčivý lektvar (8 HP)
- Lektvar vánoční pohody (12 HP)
- Elixír vánočního ducha (20 HP)
- Vaječný lektvar (35 HP)

### Bossové
- Vánoční skřítek (30 HP, 8 DMG)
- Vánoční skřet (50 HP, 10 DMG)
- Strážce vánočního města (80 HP, 20 DMG)
- Perníkový golem (110 HP, 25 DMG)
- Ledová královna (150 HP, 30 DMG)
- Krampus (200 HP, 40 DMG)
- Santa (250 HP, 50 DMG)

## Jak hrát

1. Začněte novou hru nebo načtěte existující uloženou hru
2. Navigujte v menu pomocí šipek a Enteru
3. Bojujte s bossy a získávejte mince
4. Kupujte lepší vybavení v obchodě
5. Pokuste se porazit všechny bossy

## Technické požadavky

- .NET 8.0
- Konzolová aplikace
- Windows OS

## Struktura projektu

- `Program.cs` - Hlavní herní smyčka a systém menu
- `invMng.cs` - Systém správy inventáře
- `game.cs` - Systém soubojů a bossů
- `screen.cs` - Systém zobrazení a uživatelského rozhraní
- `saveMng.cs` - Systém ukládání/nahrávání

## Sestavení ze zdroje

```sh
dotnet build
dotnet run
```