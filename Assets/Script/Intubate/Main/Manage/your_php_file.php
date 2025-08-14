<?php
header('Content-Type: text/plain');

$servername = "localhost";
$username = "root";
$password = "12qwaszx1qaz2wsx";
$dbname = "hpds_anne";
$port = "3306";

// 設置 PHP 時區
date_default_timezone_set('Asia/Taipei');

// 建立連接
$conn = new mysqli($servername, $username, $password, $dbname, $port);

// 檢查連接
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// 接收 Unity 傳遞的參數
$student_id = $_POST['student_id'] ?? null;
$score = $_POST['score'] ?? null;
$incorrect_questions = $_POST['incorrect_questions'] ?? null;
$answer_time = $_POST['answer_time'] ?? null;
$answer_date = $_POST['answer_date'] ?? null;

// 將 'T' 替換回空格並檢查格式
if ($answer_date) {
    $answer_date = str_replace('T', ' ', $answer_date);
}

// 驗證輸入
if (!$student_id || !$score || !$incorrect_questions || !$answer_time || !$answer_date) {
    echo "Missing required parameters.";
    exit;
}

// 插入資料
$stmt = $conn->prepare("INSERT INTO score (student_id, score, incorrect_questions, answer_time, answer_date) VALUES (?, ?, ?, ?, ?)");
if (!$stmt) {
    error_log("Prepare statement failed: " . $conn->error);
    echo "Error: " . $conn->error;
    $conn->close();
    exit;
}

$stmt->bind_param("sssss", $student_id, $score, $incorrect_questions, $answer_time, $answer_date);

if ($stmt->execute()) {
    echo "New record created successfully";
} else {
    error_log("MySQL Error: " . $stmt->error);
    echo "Error: " . $stmt->error;
}

$stmt->close();
$conn->close();
?>