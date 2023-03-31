# LEdit

This repository contains the source code for the ancient, unfinished application *LEdit*.

LEdit was a ROM editor for the SNES game *Lufia 2 - Rise of the Sinistrals*. The project started in 2006 and was never really finished, nor really released. However, the source code contains a lot knowledge about the structure of the game ROM &ndash; beyond the infamous Reliquary. As I appear to hold a monopoly on some of this knowledge, I decided to release this source code into the public domain so it is available for everyone.

All my research was on the German ROM of Lufia 2. While data structures and algorithms do work for other language versions, parts of the source code contain hardcoded addresses into the German ROM.

## Structure

The source code comes in two fragments. Initially, the project was started in C#. Eventually, for some reason I cannot even remember, I started switching to Java. Somewhere in the rewrite progress, development stopped altogether. I know this is a huge mess, but it is what it is now. By rule of thumb, if something is available in the Java version, it is easier to read and implemented in a cleaner way. However, the C# version was packed with much more information because it was there first.

Both projects are potentially defunct, I have not tried to compile them in a very long time. Trying to get them to run may be worth it, however! Please open a pull request if you manage to do so.

### Feature Comparison

This is an attempt at a feature comparison between the C# and Java versions. It may be incomplete, and there are no guarantees.

A :heavy_check_mark: marks that it's supported, a :x: means not at all. Later, I use icons to indicate that something :open_file_folder: can be loaded, :eye: can be displayed in a GUI and :floppy_disk: can be written back into the ROM.

| Feature                                                      | C#                                   | Java                                          |
| ------------------------------------------------------------ | ------------------------------------ | --------------------------------------------- |
| Support for the German ROM                                   | :heavy_check_mark:                   | :heavy_check_mark:                            |
| Support for the English ROM                                  | :x:                                  | :heavy_check_mark:                            |
| Easy extensibility for other languages <sup>1</sup>          | :x:<br />(hardcoded addresses)       | :heavy_check_mark:<br />(abstract base class) |
| Decompression and extraction of [Compressed Files](#Compressed-Files) | :heavy_check_mark:                   | :heavy_check_mark:                            |
| ExLoROM conversion for more space                            | :heavy_check_mark:                   | :heavy_check_mark:                            |
| Hack for the ROM to be able to load uncompressed files       | :heavy_check_mark:                   | :heavy_check_mark:                            |
| Re-writing of (modified) uncompressed files into the ROM <sup>2</sup> | :heavy_check_mark:<br />(patching)   | :heavy_check_mark:<br />(recompilation)       |
| Checksum computation for modified ROM                        | :x:                                  | :heavy_check_mark:                            |
| Monster data                                                 | :open_file_folder::eye::floppy_disk: | :open_file_folder::eye::floppy_disk:          |
| Monster battle sprites                                       | :open_file_folder::eye::floppy_disk: | :open_file_folder::floppy_disk:               |
| Map sprites (monsters & characters)                          | :open_file_folder::eye::floppy_disk: | :x:                                           |
| Monster group ("horde") data                                 | :open_file_folder:                   | :x:                                           |
| Item data                                                    | :open_file_folder:                   | :x:                                           |
| Battle scripts <sup>3</sup>                                  | :open_file_folder::eye:              | :x:                                           |
| IP attack data                                               | :open_file_folder:                   | :x:                                           |
| Spell data                                                   | :open_file_folder:                   | :x:                                           |
| Map tilesets                                                 | :open_file_folder:                   | :x:                                           |
| Region maps                                                  | :open_file_folder::eye::floppy_disk: | :x:                                           |
| World maps                                                   | :open_file_folder::eye:              | :x:                                           |

Footnotes:

1. The C# version has addresses of the German ROM hardcoded, whereas the Java version was started with more extensibility in mind.
2. In the C# version, re-inserting files is a rather complicated process of patching the ROM file. In the Java version, the idea is to re-compile the entire ROM, which works much easier and allows for checksum computation.
3. Lufia 2 features a scripting language for items when being used in a battle. I did some progress decoding it, and implemented a partially functional disassembler in the C# version.

### Documents

In this repository, I am also sharing some documents I found back. Most of these seem to be tables &ndash; may they be helpful to someone! Again, keep in mind most research was done on the German ROM, so absolute addresses do not work for other languages. Furthermore, comments seem to be mixed between German and English.

## Concepts

I will try to recap some of the key concepts that LEdit followed. This should help understand how it operated, and to get a heads-up for trying and reading the source code.

### Compressed Files

The actual data for most of Lufia 2's content is stored in 679 compressed files located in the ROM. There is a master table containing pointers to each of the files.

My strategy for analyzing these files was to extract them from the ROM and store them as uncompressed files on the file system. The source code for decompression looks like a lot of black magic &ndash; I translated it more or less directly from the 65816 disassembly. (Now, these many years later, I can tell you that it's simply some kind of Lempel-Ziv compression, and the code could probably be written up in a much more concise way.)

Of course, the decompressed files are still just binary data. For many of them, I could figure out what they were used for. The file `C#/default.xml` contains an overview (`<fileinfos>`). Files that I gave the same extension also share the same format. I cannot possibly give any details about these formats anymore, but the source code should contain all the information I had back in the day, and then there may be more in the documents.

### LPatch

It's one thing to extract and decompress the files, what we really want to do is *edit* them. There were two problems regarding that: I had no idea about the compression scheme, so I didn't know how to compress files back. Furthermore, the ROM was jam-packed and there was not enough space to store uncompressed files, or larger files even if they would have been compressed.

To address this issue, LEdit patched the ROM, called *LPatch*. It modifies the ROM in the following ways:

* It makes the ROM an ExLoROM, giving us 4 MB of additional space.
* It patches the code for loading files so the game is able to load uncompressed files. This way I didn't have to worry about compressing them back.
* The file table is moved to a new location in the extra space, so it can be modified and potentially extended.
* It computes the correct checksum for the modified ROM so emulators won't complain (Java version only).

Data tables are moved to the extra space to hold modifications. This is very different between the C# and Java versions. To put it briefly, the C# version is an absolute mess, while I tried it to do in much cleaner ways in the Java version. The Java version basically gathers all the modifications in a Lufia 2 hacking project, and then you could simply recompile the game's ROM with the patch applied. This is really how I recommend to go about a potential followup project.

## Credits

The following people made this project possible.

- **Bernard "Reli" Algain**
  Created the *Reliquary*, a vast compendium about Lufia II ROM knowledge. Without these documents, some obstacles in the ROM research process would have taken a lot more time to overcome.
- **Universal**
  Contributed a few very useful internal format tables that accelerated the progress, especially related to item editing.
- **Patrick "pdinklag" Dinklage**
  Yours truly. I found an extraction algorithm for the compressed Lufia II resource files and created a hack that allows the game to read uncompressed files, researched the map format among a lot of other things and ultimately created LEdit.
