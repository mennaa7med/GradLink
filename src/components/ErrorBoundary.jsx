import React from 'react';

class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null, errorInfo: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true };
  }

  componentDidCatch(error, errorInfo) {
    this.setState({ error, errorInfo });
    // You can also log the error to an error reporting service here
    console.error('ErrorBoundary caught an error:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div style={styles.container}>
          <div style={styles.card}>
            <div style={styles.icon}>😵</div>
            <h1 style={styles.title}>Oops! Something went wrong</h1>
            <p style={styles.message}>
              We're sorry, but something unexpected happened. Please try refreshing the page.
            </p>
            {this.state.error && (
              <details style={styles.details}>
                <summary style={styles.summary}>Error Details</summary>
                <pre style={styles.pre}>
                  {this.state.error.toString()}
                  {this.state.errorInfo?.componentStack}
                </pre>
              </details>
            )}
            <div style={styles.buttons}>
              <button
                style={styles.button}
                onClick={() => window.location.reload()}
              >
                🔄 Refresh Page
              </button>
              <button
                style={{ ...styles.button, ...styles.secondaryButton }}
                onClick={() => window.location.href = '/'}
              >
                🏠 Go Home
              </button>
            </div>
          </div>
        </div>
      );
    }

    return this.props.children;
  }
}

const styles = {
  container: {
    minHeight: '100vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
    padding: '20px',
  },
  card: {
    background: 'white',
    borderRadius: '20px',
    padding: '50px',
    maxWidth: '500px',
    textAlign: 'center',
    boxShadow: '0 20px 60px rgba(0, 0, 0, 0.3)',
  },
  icon: {
    fontSize: '80px',
    marginBottom: '20px',
  },
  title: {
    color: '#333',
    marginBottom: '15px',
    fontSize: '24px',
  },
  message: {
    color: '#666',
    marginBottom: '25px',
    lineHeight: '1.6',
  },
  details: {
    textAlign: 'left',
    marginBottom: '25px',
    background: '#f5f5f5',
    padding: '15px',
    borderRadius: '10px',
  },
  summary: {
    cursor: 'pointer',
    color: '#666',
    fontWeight: '600',
  },
  pre: {
    fontSize: '12px',
    overflow: 'auto',
    maxHeight: '200px',
    marginTop: '10px',
    color: '#dc2626',
  },
  buttons: {
    display: 'flex',
    gap: '15px',
    justifyContent: 'center',
  },
  button: {
    padding: '15px 25px',
    background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
    color: 'white',
    border: 'none',
    borderRadius: '10px',
    fontSize: '16px',
    fontWeight: '600',
    cursor: 'pointer',
  },
  secondaryButton: {
    background: '#f0f0f0',
    color: '#333',
  },
};

export default ErrorBoundary;

















