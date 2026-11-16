# FHM Crawler (http://freehardmusic.com/) Change Log 📋

## v2.0 *(current)* 🆕

#### 🎨 User interface:
 - Redesigned the main window with a clean and consistent layout: a header with the navigation and the theme switch, a search card, a results card with its own toolbar, and a status bar.
 - Added a dark theme (graphite with a wine red accent) and a light theme (default Windows colors), switchable at any time with the "Dark theme" switch.
 - The chosen theme is saved in the user settings and restored on the next start. On the first start, the program uses the Windows app theme.
 - The window title bar is darkened when the dark theme is applied (Windows 10 version 1809 or later).
 - New flat controls without the white borders of the standard Windows controls: buttons, text box, drop-down lists, album list, theme switch and progress bar.
 - The tabs were replaced by "Search" and "Browser (log in)" views, and the artist and genre/country/year search modes by a segmented selector that only shows the fields of the selected mode.
 - The album list shows a sort indicator in the sorted column, alternating row colors, and a message while it is empty; the "Download links" column fills the available width.
 - The status bar shows the progress of the search page by page. The progress bar keeps its color while a search runs, and turns yellow when the search is cancelled or red when it fails.
 - Added a "Go to home page" button to the browser view.
 - Added a GitHub button to the header that opens the repository of the program in the default web browser.
 - Switching the theme, the view or the search mode redraws the window at once, without white flashing.
 - Minimum window size adjusted to the new layout.

#### 🔄 Changes:
 - The "Copy all as CSV" button now uses the semicolon (;) as field delimiter instead of the comma.
 - Artist names and album titles never contain the CSV delimiter: semicolons are replaced with commas, and line breaks with white-spaces.
 - The year list is now generated dynamically, from 1937 to the current year, instead of being a fixed list that ended in 2027 and skipped some years.

#### 🛠️ Fixes:
 - Resolved an issue where every search kept using the session cookies of the first search, so logging in again in the browser had no effect until the program was restarted.

#### 🌟 Improvements:
 - The artist name is trimmed before searching.
 - Extended the unit test project to cover the CSV format, the year list, the themes and the new controls.

## v1.2.1 🔄

#### 🛠️ Fixes:
 - Resolved an issue where an extra, empty results page was requested when the number of found albums was an exact multiple of 10.
 - Resolved a crash (NullReferenceException) when a search results page contained no albums.
 - Resolved a crash (ArgumentOutOfRangeException) when a crawled page yielded no albums to add to the list.
 - Resolved an issue where, after a network error during a search, every new search was rejected with the message "Another fetch operation is already running in background" until the program was restarted.
 - Resolved an issue where sorting a text column after a numeric or date column kept using the numeric/date comparison rules.
 - Resolved an issue where the text column sorter ignored its own rules for empty values, falling back to the default comparison.
 - The "Copy All Items As CSV" button now escapes values that contain commas, double quotes or line breaks, so they no longer break the CSV columns.
 - Resolved a crash (ArgumentOutOfRangeException) when building a query string from an empty parameter collection.
 - Resolved an issue where the network resources used by the crawler were not released when the program was closed.

#### 🌟 Improvements:
 - Albums whose download links cannot be retrieved (network error or malformed response) are skipped as before, but any other unexpected error is no longer silently ignored.
 - The album identifier is now extracted from the album links in a more robust way.
 - The crawler events are raised on the UI thread, while the network and parsing work no longer runs on it.
 - Reworked the crawler API: the search query now provides its URL and validates that an artist name is not combined with country, genre or year criteria; the album information now includes the album identifier and its URL; the crawl events now provide the search query and the search page index.
 - Extended the XML documentation of the crawler classes, including a usage example.
 - Added a unit test project (MSTest) that covers the crawler (search queries, HTML/XML parsing, paging, cancellation and events, using a fake web server instead of real network access), the list view sorting and CSV export, the query string builder, the comparers, the tab control extensions, the cookie-aware web client and the search filter data sources.
 - Minor code changes and optimizations.

## v1.2.0 🔄

#### 🛠️ Fixes:
 - Resolved an authentication error preventing successful log-in access to the FHM website.
 - Resolved an issue where the year list was capped at 2018, preventing the retrieval of content from recent years.

## v1.1 🔄

#### 🛠️ Fixes:
 - The program has stopped working due breaking changes on the FHM server. Crawler logic has been updated.

#### 🚀 New Features:
 - Added a "Browser (Login)" tab to log in to the FHM website. A log-in is required (only once) for the crawler to work.

#### 🌟 Improvements:
 - Source-code now targets .NET Framework 4.8
 - Default user-interface font changed to Segoe UI (10pt)
 - Added a default icon for the executable file.
 - Minor code changes and optimizations.
 
## v1.0 🔄
Initial Release.