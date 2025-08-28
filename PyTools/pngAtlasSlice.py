from PIL import Image
import os

def auto_slice_by_alpha(image_path, output_dir, min_size=10):
    image = Image.open(image_path).convert("RGBA")
    pixels = image.load()
    width, height = image.size

    visited = [[False]*height for _ in range(width)]
    os.makedirs(output_dir, exist_ok=True)

    def is_transparent(x, y):
        return pixels[x, y][3] == 0  # alpha值为0

    def flood_fill(x, y, rect):
        stack = [(x, y)]
        while stack:
            cx, cy = stack.pop()
            if cx < 0 or cy < 0 or cx >= width or cy >= height:
                continue
            if visited[cx][cy] or is_transparent(cx, cy):
                continue
            visited[cx][cy] = True
            rect[0] = min(rect[0], cx)
            rect[1] = min(rect[1], cy)
            rect[2] = max(rect[2], cx)
            rect[3] = max(rect[3], cy)
            stack.extend([
                (cx+1, cy), (cx-1, cy), (cx, cy+1), (cx, cy-1)
            ])

    count = 0
    for y in range(height):
        for x in range(width):
            if not visited[x][y] and not is_transparent(x, y):
                rect = [x, y, x, y]
                flood_fill(x, y, rect)
                x0, y0, x1, y1 = rect
                if (x1 - x0) > min_size and (y1 - y0) > min_size:
                    sprite = image.crop((x0, y0, x1+1, y1+1))
                    sprite.save(os.path.join(output_dir, f"icon_{count:03d}.png"))
                    count += 1

    # print(f"✅ 自动提取 {count} 个图标，保存在 {output_dir}")

# 示例使用
auto_slice_by_alpha("atlas.png", "output_auto", min_size=10)
