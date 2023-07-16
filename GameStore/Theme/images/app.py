import cv2
import os

def resize_images_in_directory(directory, new_size):
    dem = 0
    for filename in os.listdir(directory):
        if filename.endswith(".jpg") or filename.endswith(".png"):
            image_path = os.path.join(directory, filename)
            try:
                image = cv2.imread(image_path)
                if image is None:
                    print("Failed to read image:", image_path)
                    continue
                resized_image = cv2.resize(image, new_size)
                cv2.imwrite(image_path, resized_image)
                print("Resized Image:", dem)
                dem += 1
            except Exception as e:
                print("Error processing image:", image_path)
                print("Error message:", str(e))

# Đường dẫn tới thư mục chứa các hình ảnh cần thay đổi kích thước
directory_path = r"C:\Users\NC\Desktop\Workspace\QLDACNTT\GameStore\Theme\images\ImageResource"

# Kích thước mới (đơn vị: pixel)
new_size = (250, 250)

resize_images_in_directory(directory_path, new_size)


# from unidecode import unidecode
# import os

# def normalize_filenames_in_directory(directory):
#     for filename in os.listdir(directory):
#         normalized_filename = unidecode(filename)
#         original_filepath = os.path.join(directory, filename)
#         normalized_filepath = os.path.join(directory, normalized_filename)
#         os.rename(original_filepath, normalized_filepath)

# # Đường dẫn tới thư mục chứa các tệp tin cần chuẩn hóa
# directory_path = r"C:\Users\NC\Desktop\Workspace\QLDACNTT\GameStore\Theme\images\ImageResource"

# normalize_filenames_in_directory(directory_path)
