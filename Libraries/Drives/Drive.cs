/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using Cube.Generics;

namespace Cube.FileSystem {
    /* --------------------------------------------------------------------- */
    ///
    /// DriveType
    /// 
    /// <summary>
    /// ドライブの種類を定義した列挙型です。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public enum DriveType : uint
    {
        Unknown       = 0,
        CD            = 1,
        Dvd           = 2,
        FloppyDisk    = 3,
        HardDisk      = 4,
        Network       = 5,
        RemovableDisk = 6
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Drive
    /// 
    /// <summary>
    /// ドライブに関する情報を保持するためのクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// 現在、API で取得可能な情報の一部を保持しています。プロパティは
    /// 将来的に増減する可能性があります。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Drive
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Drive
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="letter">
        /// ドライブレターを表す A-Z のアルファベット
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public Drive(char letter) : this($"{letter}:") { }

        /* ----------------------------------------------------------------- */
        ///
        /// Drive
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="letter">ドライブレター</param>
        ///
        /// <remarks>
        /// 引数には、C: のようにコロン付ドライブレターを指定します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Drive(string letter)
        {
            GetInfo(letter);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Drive
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="obj">ドライブを表すオブジェクト</param>
        /// 
        /// <remarks>
        /// 引数には、Win32_LogicalDisk から取得したオブジェクトを指定
        /// します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Drive(object obj)
        {
            var drive = obj as ManagementObject;
            if (drive == null) throw new ArgumentException("Invalid object");
            GetInfo(drive);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// ドライブ番号を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Index { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Letter
        ///
        /// <summary>
        /// ドライブレターを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Letter { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// VolumeLabel
        ///
        /// <summary>
        /// ボリュームラベルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string VolumeLabel { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Type
        ///
        /// <summary>
        /// ドライブの種類を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DriveType Type { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// ドライブのフォーマットを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Format { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// ディスクのモデル名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Model { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Interface
        ///
        /// <summary>
        /// インターフェースの種類を取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// "SCSI", "HDC", "IDE", "USB", "1394" のいずれかの値が設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Interface { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FreeSpace
        ///
        /// <summary>
        /// ディスクの空き容量を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ulong FreeSpace { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        ///
        /// <summary>
        /// ディスク容量を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ulong Size { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// ドライブを取り外します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach()
        {
            var device = new Device(this);
            device.Detach();
        }

        #endregion

        #region Static methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetDrives
        ///
        /// <summary>
        /// 現在のドライブ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Drive> GetDrives()
        {
            using (var mos = new ManagementObjectSearcher("Select * From Win32_LogicalDisk"))
            {
                return mos.Get()
                          .Cast<ManagementObject>()
                          .Select(obj => new Drive(obj))
                          .Where(drive => !string.IsNullOrEmpty(drive.Letter));
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetInfo
        ///
        /// <summary>
        /// 必要な情報を取得して、プロパティを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void GetInfo(string letter)
        {
            var query = $"Select * From Win32_LogicalDisk Where DeviceID = '{letter}'";
            using (var mos = new ManagementObjectSearcher(query))
            {
                var results = mos.Get();
                if (results.Count <= 0) throw new ArgumentException("Drive not found");

                foreach (ManagementObject drive in results)
                {
                    GetInfo(drive);
                    break;
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetInfo
        ///
        /// <summary>
        /// 必要な情報を取得して、プロパティを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void GetInfo(ManagementObject drive)
        {
            GetDriveInfo(drive);
            GetDeviceInfo(drive);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDriveInfo
        ///
        /// <summary>
        /// ドライブに関する情報を取得して、プロパティを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void GetDriveInfo(ManagementObject drive)
        {
            Letter      = drive["Name"] as string;
            VolumeLabel = ToVolumeLabel(drive["VolumeName"], drive["Description"]);
            Type        = ToDriveType(drive["DriveType"], drive["MediaType"]);
            Format      = drive["FileSystem"] as string;
            Size        = drive["Size"].TryCast<ulong>();
            FreeSpace   = drive["FreeSpace"].TryCast<ulong>();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDeviceInfo
        ///
        /// <summary>
        /// デバイスに関する情報を取得して、プロパティを初期化します。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: HardDisk 以外のデバイス情報の取得方法を実装する。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        private void GetDeviceInfo(ManagementObject drive)
        {
            switch (Type)
            {
                case DriveType.CD:
                case DriveType.Dvd:
                    // GetCdInfo(drive);
                    break;
                case DriveType.HardDisk:
                case DriveType.Network:
                case DriveType.RemovableDisk:
                    GetHardDiskInfo(drive);
                    break;
                case DriveType.Unknown:
                    break;
                default:
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDeviceInfo
        ///
        /// <summary>
        /// HDD に関する情報を取得して、プロパティを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void GetHardDiskInfo(ManagementObject drive)
        {
            foreach (ManagementObject partition in drive.GetRelated("Win32_DiskPartition"))
            foreach (ManagementObject device in partition.GetRelated("Win32_DiskDrive"))
            {
                Index     = device["Index"].TryCast(uint.MaxValue);
                Model     = device["Model"] as string;
                Interface = device["InterfaceType"] as string;
                if (Type == DriveType.HardDisk && Interface == "USB") Type = DriveType.RemovableDisk;
                break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToVolumeLabel
        ///
        /// <summary>
        /// ボリュームラベルを表す文字列に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string ToVolumeLabel(object name, object description)
        {
            var s1 = name as string;
            var s2 = description as string;
            return !string.IsNullOrEmpty(s1) ? s1 : s2;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToDriveType
        ///
        /// <summary>
        /// DriveType 列挙型に変換します。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: USB 接続のハードディスクも 3 (Local Disk) に指定されている
        /// ※ USB 接続のフラッシュメモリは（多分）2。
        /// この辺りを他の情報も勘案しながら最適な DriveType へ割り当てる。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private DriveType ToDriveType(object drive, object media)
        {
            var index = drive.TryCast(0u);

            switch (index)
            {
                case 0: // Unknown
                    return DriveType.Unknown;
                case 1: // No Root Directory
                    return DriveType.Unknown;
                case 2: // Removable Disk
                    var dest = ToDriveType(media);
                    return (dest != DriveType.Unknown) ? dest : DriveType.RemovableDisk;
                case 3: // Local Disk
                    return DriveType.HardDisk;
                case 4: // Network Drive
                    return DriveType.Network;
                case 5: // Compact Disc
                    return DriveType.CD;
                case 6: // RAM Disk
                    return DriveType.Unknown;
                default:
                    return DriveType.Unknown;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToDriveType
        ///
        /// <summary>
        /// DriveType 列挙型に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private DriveType ToDriveType(object obj)
        {
            var index = obj.TryCast(0u);

            switch (index)
            {
                case 0: // Unknown
                    return DriveType.Unknown;
                case 1: // 5 1/4-Inch Floppy Disk - 1.2 MB - 512 bytes/sector
                case 2: // 3 1/2-Inch Floppy Disk - 1.44 MB -512 bytes/sector
                case 3: // 3 1/2-Inch Floppy Disk - 2.88 MB - 512 bytes/sector
                case 4: // 3 1/2-Inch Floppy Disk - 20.8 MB - 512 bytes/sector
                case 5: // 3 1/2-Inch Floppy Disk - 720 KB - 512 bytes/sector
                case 6: // 5 1/4-Inch Floppy Disk - 360 KB - 512 bytes/sector
                case 7: // 5 1/4-Inch Floppy Disk - 320 KB - 512 bytes/sector
                case 8: // 5 1/4-Inch Floppy Disk - 320 KB - 1024 bytes/sector
                case 9: // 5 1/4-Inch Floppy Disk - 180 KB - 512 bytes/sector
                case 10: // 5 1/4-Inch Floppy Disk - 160 KB - 512 bytes/sector
                    return DriveType.FloppyDisk;
                case 11: // Removable media other than floppy
                    return DriveType.RemovableDisk;
                case 12: // Fixed hard disk media
                    return DriveType.HardDisk;
                case 13: // 3 1/2-Inch Floppy Disk - 120 MB - 512 bytes/sector
                case 14: // 3 1/2-Inch Floppy Disk - 640 KB - 512 bytes/sector
                case 15: // 5 1/4-Inch Floppy Disk - 640 KB - 512 bytes/sector
                case 16: // 5 1/4-Inch Floppy Disk - 720 KB - 512 bytes/sector
                case 17: // 3 1/2-Inch Floppy Disk - 1.2 MB - 512 bytes/sector
                case 18: // 3 1/2-Inch Floppy Disk - 1.23 MB - 1024 bytes/sector
                case 19: // 5 1/4-Inch Floppy Disk - 1.23 MB - 1024 bytes/sector
                case 20: // 3 1/2-Inch Floppy Disk - 128 MB - 512 bytes/sector
                case 21: // 3 1/2-Inch Floppy Disk - 230 MB - 512 bytes/sector
                case 22: // 8-Inch Floppy Disk - 256 KB - 128 bytes/sector
                    return DriveType.FloppyDisk;
                default:
                    return DriveType.Unknown;
            }
        }

        #endregion
    }
}
