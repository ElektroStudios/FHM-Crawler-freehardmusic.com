# FHM Crawler (http://freehardmusic.com/) Change Log 📋

## v1.2.1 *(current)* 🆕

#### 🛠️ Fixes:
 - Resolved an issue where an extra, empty results page was requested when the number of found albums was an exact multiple of 10.
 - Resolved a crash (NullReferenceException) when a search results page contained no albums.
 - Resolved a crash (ArgumentOutOfRangeException) when a crawled page yielded no albums to add to the list.
 - Resolved an issue where, after a network error during a search, every new search was rejected with the message "Another fetch operation is already running in background" until the program was restarted.
 - Resolved an issue where sorting a text column after a numeric or date column kept using the numeric/date comparison rules.
 - Resolved an issue where the text column sorter ignored its own rules for empty values, falling back to the default comparison.
 - The "Copy All Items As CSV" button now escapes values that contain commas, double quotes or line breaks, so they no longer break the CSV columns.
 - Resolved a crash (ArgumentOutOfRangeException) when building a query string from an empty parameter collection.

#### 🌟 Improvements:
 - Albums whose download links cannot be retrieved (network error or malformed response) are skipped as before, but any other unexpected error is no longer silently ignored.
 - The album identifier is now extracted from the album links in a more robust way.
 - The crawler events are raised on the UI thread, while the network and parsing work no longer runs on it.
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