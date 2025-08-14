<?php
$servername = "fgserver2.ddns.net";
$username = "root";
$password = "12qwaszx1qaz2wsx";
$dbname = "hpds_anne";

// 建立 MySQL 連線
$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die(json_encode(["success" => false, "message" => "資料庫連線失敗"]));
}

// 獲取請求動作
$action = $_POST['action'];

switch ($action) {
    case 'add':
        $student_id = $_POST['student_id'];
        $name = base64_encode($_POST['name']); // Base64 編碼名字
        $password = $_POST['password'];
        $permission = $_POST['permission'];
        $gender = $_POST['gender'];

        $checkQuery = $conn->prepare("SELECT * FROM users WHERE student_id = ?");
        $checkQuery->bind_param("s", $student_id);
        $checkQuery->execute();
        if ($checkQuery->get_result()->num_rows > 0) {
            echo json_encode(["success" => false, "message" => "學號已存在"]);
        } else {
            $query = $conn->prepare("INSERT INTO users (student_id, name, password, permission, gender) VALUES (?, ?, ?, ?, ?)");
            $query->bind_param("sssss", $student_id, $name, $password, $permission, $gender);
            $query->execute();
            echo json_encode(["success" => true, "message" => "用戶新增成功"]);
        }
        break;

    case 'fetch':
        $result = $conn->query("SELECT * FROM users");
        $users = [];
        while ($row = $result->fetch_assoc()) {
            $row['name'] = base64_decode($row['name']); // Base64 解碼名字
            $users[] = $row;
        }
        echo json_encode(["success" => true, "data" => $users]);
        break;

    case 'delete':
        $student_id = $_POST['student_id'];
        $query = $conn->prepare("DELETE FROM users WHERE student_id = ?");
        $query->bind_param("s", $student_id);
        $query->execute();
        echo json_encode(["success" => true, "message" => "用戶刪除成功"]);
        break;

    case 'update':
        $student_id = $_POST['student_id'];
        $name = base64_encode($_POST['name']); // Base64 編碼名字
        $password = $_POST['password'];
        $permission = $_POST['permission'];
        $gender = $_POST['gender'];

        $query = $conn->prepare("UPDATE users SET name = ?, password = ?, permission = ?, gender = ? WHERE student_id = ?");
        $query->bind_param("sssss", $name, $password, $permission, $gender, $student_id);
        $query->execute();
        echo json_encode(["success" => true, "message" => "用戶資料更新成功"]);
        break;

    default:
        echo json_encode(["success" => false, "message" => "未知操作"]);
}

$conn->close();
?>
