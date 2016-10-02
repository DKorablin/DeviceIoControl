DeviceIoControl assembly
===============

Wrapper for DeviceIoControl function. Primary task of this assembly was to read SMART data from supported devices.
S.M.A.R.T.rar contains original source code written by Andrew I. Reshin (11.02.2001) in C++

Supported messages:
<ul>
    <li>IOCTL_VOLUME
        <ul>
            <li>GET_VOLUME_DISK_EXTENTS</li>
            <li>IS_CLUSTERED</li>
        </ul>
    </li>
    <li>IOCTL_DISC
        <ul>
            <li>PERFORMANCE</li>
            <li>IS_WRITABLE</li>
            <li>PERFORMANCE_OFF</li>
            <li>SMART_GET_VERSION</li>
            <li>SMART_SEND_DRIVE_COMMAND</li>
            <li>SMART_RCV_DRIVE_DATA</li>
            <li>GET_DRIVE_GEOMETRY_EX</li>
        </ul>
    </li>
    <li>IOCTL_STORAGE
        <ul>
            <li>CHECK_VERIFY</li>
            <li>CHECK_VERIFY2</li>
            <li>MEDIA_REMOVAL</li>
            <li>EJECT_MEDIA</li>
            <li>GET_MEDIA_TYPES_EX</li>
            <li>GET_MEDIA_SERIAL_NUMBER</li>
            <li>GET_HOTPLUG_INFO</li>
            <li>GET_DEVICE_NUMBER</li>
            <li>PREDICT_FAILURE</li>
            <li>QUERY_PROPERTY</li>
        </ul>
    </li>
    <li>FSCTL
        <ul>
            <li>LOCK_VOLUME</li>
            <li>UNLOCK_VOLUME</li>
            <li>DISMOUNT_VOLUME</li>
            <li>IS_VOLUME_MOUNTED</li>
            <li>FILESYSTEM_GET_STATISTICS</li>
            <li>GET_NTFS_VOLUME_DATA</li>
            <li>GET_VOLUME_BITMAP</li>
        </ul>
    </li>
</ul>
