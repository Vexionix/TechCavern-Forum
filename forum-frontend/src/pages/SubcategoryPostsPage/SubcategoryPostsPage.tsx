import {
  useParams,
  useNavigate,
  useSearchParams,
  Link,
} from "react-router-dom";
import { useState, useEffect } from "react";
import api from "../../utils/api";
import Pagination from "@mui/material/Pagination";

import "./SubcategoryPostsPage.css";
import { FaLock } from "react-icons/fa";
import { BsPinFill } from "react-icons/bs";
import timeAgo from "../../utils/timeAgo";

interface Post {
  id: number;
  title: string;
  content: string;
  views: number;
  commentsCount: number;
  userId: number;
  latestCommentUserId: number;
  subcategoryId: number;
  username: string;
  latestCommenterUsername: string;
  isPinned: boolean;
  isLocked: boolean;
  createdAt: string;
  latestCommentPostedAt: string;
}

const SubcategoryPostsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [pinnedPosts, setPinnedPosts] = useState<Post[]>([]);
  const [regularPosts, setRegularPosts] = useState<Post[]>([]);
  const [searchParams, setSearchParams] = useSearchParams();
  const [maxPagesCount, setMaxPagesCount] = useState<number>(1);
  const [page, setPage] = useState<number>(
    Number(searchParams.get("page")) || 1
  );
  const navigate = useNavigate();

  useEffect(() => {
    updateToValidPage();
    getPosts();
    setSearchParams({ page: page.toString() }, { replace: true });
  }, [id, page]);

  useEffect(() => {
    const pageFromQuery = Number(searchParams.get("page"));
    if (pageFromQuery && pageFromQuery !== page) {
      setPage(pageFromQuery);
    }
  }, [searchParams]);

  const updateToValidPage = async () => {
    try {
      const response = await api.get(`/subcategories/${id}/max-pages`);
      if (response.data === 0) setMaxPagesCount(1);
      else setMaxPagesCount(response.data);
      if (page < 1 || page > maxPagesCount) {
        setPage(1);
        navigate(`/subcategory/${id}?page=${page}`, { replace: true });
      }
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching page number for subcategory:", err);
      }
    }
  };

  const getPosts = async () => {
    try {
      const response = await api.get(`/posts/subcategory/${id}?page=${page}`);
      setPinnedPosts(response.data.pinnedPosts);
      setRegularPosts(response.data.regularPosts);
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching posts for subcategory:", err);
      }
    }
  };

  const handlePageChange = (
    _event: React.ChangeEvent<unknown>,
    value: number
  ) => {
    setPage(value);
  };

  return (
    <div className="subcategory-posts-wrapper">
      <div className="subcategory-posts-container">
        <div className="pinned-posts-container">
          <div className="posts-title">
            <h3>Pinned Posts</h3>
          </div>
          {renderPosts(pinnedPosts)}
        </div>

        <div className="posts-title-and-button">
          <h3>Posts</h3>
          <Link to={`/add-post/${id}`} className="add-post-button">
            ADD POST
          </Link>
        </div>
        {renderPosts(regularPosts)}
        <div className="pagination-bar-div">
          <Pagination
            count={maxPagesCount}
            page={page}
            onChange={handlePageChange}
            showFirstButton
            showLastButton
            color="primary"
            className="pagination-bar"
          />
        </div>
      </div>
    </div>
  );
};

const renderPosts = (posts: Post[]) => {
  if (posts.length === 0)
    return (
      <div className="no-posts-found">
        <p>No posts available.</p>
      </div>
    );
  return (
    <>
      {posts.map((post, postIndex) => {
        const isLast = postIndex === posts.length - 1;
        return (
          <div key={`post-${post.id}`}>
            <div className="posts-container">
              <div className="post-content-container">
                <div className="posts-left-container">
                  {post.isPinned && (
                    <span>
                      <BsPinFill className="svg-pin" />
                      &nbsp;
                    </span>
                  )}
                  {post.isLocked && (
                    <span>
                      <FaLock className="svg-lock" />
                      &nbsp;
                    </span>
                  )}
                  <Link to={"/post/" + post.id}>{post.title}</Link>
                  <p className="post-preview">
                    {post.content.length > 75
                      ? post.content.substring(0, 75) + "..."
                      : post.content}
                  </p>
                  <div>
                    <p>
                      <span> {timeAgo(post.createdAt)} by </span>{" "}
                      <Link to={"/user/profile/" + post.userId}>
                        {post.username}
                      </Link>
                    </p>
                  </div>
                </div>
                <div className="mobile-separator-container">
                  <div className="mobile-separator"></div>
                </div>
                <div className="posts-right-container">
                  <div className="views-comments">
                    <div className="posts-views">
                      <p>Views: {post.views}</p>
                    </div>
                    <div className="posts-comments">
                      <p>Comments: {post.commentsCount}</p>
                    </div>
                  </div>
                  <div className="post-content-separator"></div>
                  <div className="latest-comment-container">
                    {post.latestCommentPostedAt != undefined ? (
                      <>
                        <p>Latest comment by </p>
                        <p>
                          <Link
                            to={"/user/profile/" + post.latestCommentUserId}
                          >
                            {post.latestCommenterUsername}
                          </Link>
                          <span> ● {timeAgo(post.latestCommentPostedAt)} </span>
                        </p>
                      </>
                    ) : (
                      <p className="no-comments-yet">No comments yet</p>
                    )}
                  </div>
                </div>
              </div>
            </div>
            {!isLast && (
              <div className="posts-separator-container">
                <p className="posts-separator"></p>
              </div>
            )}
          </div>
        );
      })}
      ;
    </>
  );
};

export default SubcategoryPostsPage;
