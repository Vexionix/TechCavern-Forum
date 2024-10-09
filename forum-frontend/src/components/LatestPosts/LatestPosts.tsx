import { useEffect, useState } from "react";
import api from "../../utils/api";

import "./latestPosts.css";
import { Link } from "react-router-dom";
import timeAgo from "../../utils/timeAgo";

interface PostData {
  id: number;
  title: string;
  username: string;
  userId: number;
  latestActivityPostedAt: string;
}

const LatestPosts = () => {
  const [posts, setPosts] = useState<PostData[]>([]);

  useEffect(() => {
    const controller = new AbortController();

    const getLatestPosts = async () => {
      try {
        const response = await api.get("/posts/latest", {
          signal: controller.signal,
        });
        setPosts(response.data);
      } catch (err: any) {
        if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
          console.error("Error fetching categories:", err);
        }
      }
    };

    getLatestPosts();

    return () => {
      controller.abort();
    };
  }, []);

  return (
    <div className="latest-posts-wrapper">
      <div className="latest-posts-title">
        <p>Latest posts activity</p>
      </div>
      <div className="latest-posts-containers-separator"></div>
      <div className="latest-posts-content"></div>
      {posts.length > 0 ? (
        posts.map((post) => {
          return (
            <div key={"post-" + post.id} className="latest-posts-element">
              <div className="latest-posts">
                <Link to={"/post/" + post.id}>{post.title}</Link>
                <div>
                  <p>
                    <Link to={"/user/profile/" + post.userId}>
                      {post.username}
                    </Link>
                    <span> ● {timeAgo(post.latestActivityPostedAt)} </span>
                  </p>
                </div>
              </div>
            </div>
          );
        })
      ) : (
        <p>No posts found</p>
      )}
    </div>
  );
};

export default LatestPosts;
